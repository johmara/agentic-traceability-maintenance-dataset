using System.Net;
using System.Net.Http.Json;
using ReferenceManager.Models;
using ReferenceManager.Requests;
using Xunit;

namespace ReferenceManager.Tests;

// &begin[Authors]
public class AuthorEndpointTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task ListAuthors_ReturnsOkWithItems()
    {
        var authors = await _client.GetFromJsonAsync<List<Author>>("/api/v1/authors");

        Assert.NotNull(authors);
        Assert.NotEmpty(authors);
    }

    [Fact]
    public async Task CreateAuthor_ValidRequest_ReturnsCreated()
    {
        var req = new AuthorRequest($"Unique Author {Guid.NewGuid()}", null, []);

        var response = await _client.PostAsJsonAsync("/api/v1/authors", req);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var author = await response.Content.ReadFromJsonAsync<Author>();
        Assert.NotNull(author);
        Assert.Equal(req.Name, author.Name);
    }

    [Fact]
    public async Task CreateAuthor_DuplicateName_ReturnsConflict()
    {
        var name = $"Conflicting Author {Guid.NewGuid()}";
        await _client.PostAsJsonAsync("/api/v1/authors", new AuthorRequest(name, null, []));

        var response = await _client.PostAsJsonAsync("/api/v1/authors", new AuthorRequest(name, null, []));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task GetAuthor_ExistingId_ReturnsOk()
    {
        var created = await _client.PostAsJsonAsync("/api/v1/authors",
            new AuthorRequest($"GetTest {Guid.NewGuid()}", null, []));
        var author = await created.Content.ReadFromJsonAsync<Author>();
        Assert.NotNull(author);

        var response = await _client.GetAsync($"/api/v1/authors/{author.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAuthor_NonExistentId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/v1/authors/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateAuthor_ExistingId_ReturnsOkWithUpdatedData()
    {
        var created = await _client.PostAsJsonAsync("/api/v1/authors",
            new AuthorRequest($"UpdateTest {Guid.NewGuid()}", null, []));
        var author = await created.Content.ReadFromJsonAsync<Author>();
        Assert.NotNull(author);

        var updateReq = new AuthorRequest("Updated Name", "updated@example.com", []);
        var response = await _client.PutAsJsonAsync($"/api/v1/authors/{author.Id}", updateReq);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await response.Content.ReadFromJsonAsync<Author>();
        Assert.Equal("Updated Name", updated?.Name);
        Assert.Equal("updated@example.com", updated?.Email);
    }

    [Fact]
    public async Task DeleteAuthor_ExistingId_ReturnsNoContent()
    {
        var created = await _client.PostAsJsonAsync("/api/v1/authors",
            new AuthorRequest($"DeleteTest {Guid.NewGuid()}", null, []));
        var author = await created.Content.ReadFromJsonAsync<Author>();
        Assert.NotNull(author);

        var response = await _client.DeleteAsync($"/api/v1/authors/{author.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task CreatePaper_SameAuthorName_ReusesSameAuthor()
    {
        var name = $"SharedAuthor {Guid.NewGuid()}";
        var req1 = new PaperRequest($"Paper A {Guid.NewGuid()}", [new AuthorRequest(name, null, [])], 2024, null, null, null, null);
        var req2 = new PaperRequest($"Paper B {Guid.NewGuid()}", [new AuthorRequest(name, null, [])], 2024, null, null, null, null);

        var r1 = await _client.PostAsJsonAsync("/api/v1/papers", req1);
        var r2 = await _client.PostAsJsonAsync("/api/v1/papers", req2);
        var p1 = await r1.Content.ReadFromJsonAsync<Paper>();
        var p2 = await r2.Content.ReadFromJsonAsync<Paper>();

        Assert.NotNull(p1); Assert.NotNull(p2);
        Assert.Single(p1.Authors); Assert.Single(p2.Authors);
        Assert.Equal(p1.Authors[0].Id, p2.Authors[0].Id);
    }
    // &begin[MergeAuthors]
    [Fact]
    public async Task MergeAuthors_ReassignesPapersToKeepAndDeletesMerge()
    {
        var keepReq = new AuthorRequest($"Keep {Guid.NewGuid()}", null, []);
        var mergeReq = new AuthorRequest($"Merge {Guid.NewGuid()}", null, []);
        var keep = await (await _client.PostAsJsonAsync("/api/v1/authors", keepReq)).Content.ReadFromJsonAsync<Author>();
        var merge = await (await _client.PostAsJsonAsync("/api/v1/authors", mergeReq)).Content.ReadFromJsonAsync<Author>();
        Assert.NotNull(keep); Assert.NotNull(merge);

        // Create a paper attributed to the merge author
        var paperReq = new PaperRequest($"Paper {Guid.NewGuid()}", [new AuthorRequest(mergeReq.Name, null, [])], 2024, null, null, null, null);
        await _client.PostAsJsonAsync("/api/v1/papers", paperReq);

        var response = await _client.PostAsync($"/api/v1/authors/{keep.Id}/merge/{merge.Id}", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<Author>();
        Assert.NotNull(result);
        Assert.Equal(keep.Id, result.Id);
        Assert.NotEmpty(result.Papers);

        // Merge author should be gone
        var getDeleted = await _client.GetAsync($"/api/v1/authors/{merge.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getDeleted.StatusCode);
    }

    [Fact]
    public async Task MergeAuthors_NoDuplicatePaperLinks_WhenBothAlreadyOnSamePaper()
    {
        var keepReq = new AuthorRequest($"KeepShared {Guid.NewGuid()}", null, []);
        var mergeReq = new AuthorRequest($"MergeShared {Guid.NewGuid()}", null, []);
        var keep = await (await _client.PostAsJsonAsync("/api/v1/authors", keepReq)).Content.ReadFromJsonAsync<Author>();
        var merge = await (await _client.PostAsJsonAsync("/api/v1/authors", mergeReq)).Content.ReadFromJsonAsync<Author>();
        Assert.NotNull(keep); Assert.NotNull(merge);

        // Paper has both authors
        var paperReq = new PaperRequest($"SharedPaper {Guid.NewGuid()}",
            [new AuthorRequest(keepReq.Name, null, []), new AuthorRequest(mergeReq.Name, null, [])],
            2024, null, null, null, null);
        await _client.PostAsJsonAsync("/api/v1/papers", paperReq);

        var response = await _client.PostAsync($"/api/v1/authors/{keep.Id}/merge/{merge.Id}", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<Author>();
        Assert.NotNull(result);
        Assert.Single(result.Papers);
    }

    [Fact]
    public async Task MergeAuthors_AppendsDifferentAffiliations()
    {
        var keepReq = new AuthorRequest($"AffKeep {Guid.NewGuid()}", null,
            [new AffiliationRequest("Bebas Uni", "SE", "Stockholm")]);
        var mergeReq = new AuthorRequest($"AffMerge {Guid.NewGuid()}", null,
            [new AffiliationRequest("Abbas Uni", "SE", "Gothenburg")]);
        var keep = await (await _client.PostAsJsonAsync("/api/v1/authors", keepReq)).Content.ReadFromJsonAsync<Author>();
        var merge = await (await _client.PostAsJsonAsync("/api/v1/authors", mergeReq)).Content.ReadFromJsonAsync<Author>();
        Assert.NotNull(keep); Assert.NotNull(merge);

        var response = await _client.PostAsync($"/api/v1/authors/{keep.Id}/merge/{merge.Id}", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<Author>();
        Assert.NotNull(result);
        Assert.Equal(2, result.Affiliations.Count);
        Assert.Contains(result.Affiliations, a => a.Name == "Bebas Uni");
        Assert.Contains(result.Affiliations, a => a.Name == "Abbas Uni");
    }

    [Fact]
    public async Task MergeAuthors_NoDuplicateAffiliations_WhenSameName()
    {
        var sharedAff = new AffiliationRequest("Same Uni", "SE", "Stockholm");
        var keepReq = new AuthorRequest($"AffDupKeep {Guid.NewGuid()}", null, [sharedAff]);
        var mergeReq = new AuthorRequest($"AffDupMerge {Guid.NewGuid()}", null, [sharedAff]);
        var keep = await (await _client.PostAsJsonAsync("/api/v1/authors", keepReq)).Content.ReadFromJsonAsync<Author>();
        var merge = await (await _client.PostAsJsonAsync("/api/v1/authors", mergeReq)).Content.ReadFromJsonAsync<Author>();
        Assert.NotNull(keep); Assert.NotNull(merge);

        var response = await _client.PostAsync($"/api/v1/authors/{keep.Id}/merge/{merge.Id}", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<Author>();
        Assert.NotNull(result);
        Assert.Single(result.Affiliations);
    }

    [Fact]
    public async Task MergeAuthors_SameId_ReturnsBadRequest()
    {
        var author = await (await _client.PostAsJsonAsync("/api/v1/authors",
            new AuthorRequest($"Self {Guid.NewGuid()}", null, []))).Content.ReadFromJsonAsync<Author>();
        Assert.NotNull(author);

        var response = await _client.PostAsync($"/api/v1/authors/{author.Id}/merge/{author.Id}", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task MergeAuthors_NonExistentId_ReturnsNotFound()
    {
        var author = await (await _client.PostAsJsonAsync("/api/v1/authors",
            new AuthorRequest($"Real {Guid.NewGuid()}", null, []))).Content.ReadFromJsonAsync<Author>();
        Assert.NotNull(author);

        var response = await _client.PostAsync($"/api/v1/authors/{author.Id}/merge/99999", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    // &end[MergeAuthors]
}
// &end[Authors]
