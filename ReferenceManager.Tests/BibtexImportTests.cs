using System.Net;
using System.Net.Http.Json;
using ReferenceManager.Responses;
using ReferenceManager.Models;
using Xunit;

namespace ReferenceManager.Tests;

public class BibtexImportTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private static MultipartFormDataContent BibContent(string bib) =>
        new() { { new StringContent(bib), "file", "refs.bib" } };

    [Fact]
    public async Task ImportBibtex_ValidEntry_ReturnsCreated1Skipped0()
    {
        var bib = """
            @article{test2024,
              title = {A Test Paper},
              author = {Smith, John and Doe, Jane},
              year = {2024},
              abstract = {Some abstract text.},
              doi = {10.9999/bibtex-import-test-unique},
              journal = {Journal of Testing}
            }
            """;

        var response = await _client.PostAsync("/api/v1/papers/import/bibtex", BibContent(bib));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ImportResult>();
        Assert.NotNull(result);
        Assert.Equal(1, result.Created);
        Assert.Equal(0, result.Skipped);
    }

    [Fact]
    public async Task ImportBibtex_DuplicateDoi_SkipsSecondEntry()
    {
        var doi = $"10.9999/dup-test-{Guid.NewGuid()}";
        var bib = $$"""
            @article{first,
              title = {First Paper},
              author = {Author One},
              year = {2024},
              doi = {{{doi}}}
            }
            @article{second,
              title = {Second Paper with Same DOI},
              author = {Author Two},
              year = {2024},
              doi = {{{doi}}}
            }
            """;

        var response = await _client.PostAsync("/api/v1/papers/import/bibtex", BibContent(bib));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ImportResult>();
        Assert.NotNull(result);
        Assert.Equal(1, result.Created);
        Assert.Equal(1, result.Skipped);
    }

    [Fact]
    public async Task ImportBibtex_NoDoi_CreatesMultipleEntries()
    {
        var bib = """
            @inproceedings{conf1,
              title = {Conference Paper One},
              author = {Alice},
              year = {2023},
              booktitle = {Proc. of ICSE}
            }
            @inproceedings{conf2,
              title = {Conference Paper Two},
              author = {Bob},
              year = {2023},
              booktitle = {Proc. of ASE}
            }
            """;

        var response = await _client.PostAsync("/api/v1/papers/import/bibtex", BibContent(bib));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ImportResult>();
        Assert.NotNull(result);
        Assert.Equal(2, result.Created);
        Assert.Equal(0, result.Skipped);
    }

    [Fact]
    public async Task ImportBibtex_AuthorField_SplitsOnAnd()
    {
        var doi = $"10.9999/author-split-{Guid.NewGuid()}";
        var bib = $$"""
            @article{multi,
              title = {Multi-Author Paper},
              author = {Smith, John and Doe, Jane and Brown, Bob},
              year = {2024},
              doi = {{{doi}}}
            }
            """;

        await _client.PostAsync("/api/v1/papers/import/bibtex", BibContent(bib));

        var papers = await _client.GetFromJsonAsync<PagedResult<Paper>>("/api/v1/papers?limit=100");
        var imported = papers?.Items.FirstOrDefault(p => p.Doi == doi);
        Assert.NotNull(imported);
        Assert.Equal(3, imported.Authors.Count);
    }

    [Fact]
    public async Task ImportBibtex_MissingTitle_SkipsEntry()
    {
        var bib = """
            @article{notitle,
              author = {Someone},
              year = {2024}
            }
            """;

        var response = await _client.PostAsync("/api/v1/papers/import/bibtex", BibContent(bib));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ImportResult>();
        Assert.NotNull(result);
        Assert.Equal(0, result.Created);
        Assert.Equal(0, result.Skipped);
    }

    [Fact]
    public async Task ImportBibtex_NoDoi_SameTitleAndAuthors_SkipsSecondEntry()
    {
        var title = $"Duplicate Title No DOI {Guid.NewGuid()}";
        var bib = $$"""
            @inproceedings{dup1,
              title = {{{title}}},
              author = {Alice and Bob},
              year = {2023}
            }
            @inproceedings{dup2,
              title = {{{title}}},
              author = {Alice and Bob},
              year = {2023}
            }
            """;

        var response = await _client.PostAsync("/api/v1/papers/import/bibtex", BibContent(bib));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ImportResult>();
        Assert.NotNull(result);
        Assert.Equal(1, result.Created);
        Assert.Equal(1, result.Skipped);
    }

    [Fact]
    public async Task ImportBibtex_NoDoi_SameTitleDifferentAuthors_CreatesBoth()
    {
        var title = $"Same Title Different Authors {Guid.NewGuid()}";
        var bib = $$"""
            @inproceedings{v1,
              title = {{{title}}},
              author = {Alice},
              year = {2023}
            }
            @inproceedings{v2,
              title = {{{title}}},
              author = {Bob},
              year = {2024}
            }
            """;

        var response = await _client.PostAsync("/api/v1/papers/import/bibtex", BibContent(bib));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ImportResult>();
        Assert.NotNull(result);
        Assert.Equal(2, result.Created);
        Assert.Equal(0, result.Skipped);
    }

    [Fact]
    public async Task ImportBibtex_NoDoi_AlreadyInDb_SkipsEntry()
    {
        var title = $"Existing Paper No DOI {Guid.NewGuid()}";
        var bib = $$"""
            @inproceedings{existing,
              title = {{{title}}},
              author = {Carol},
              year = {2022}
            }
            """;

        // First import — creates it
        await _client.PostAsync("/api/v1/papers/import/bibtex", BibContent(bib));

        // Second import — should skip
        var response = await _client.PostAsync("/api/v1/papers/import/bibtex", BibContent(bib));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ImportResult>();
        Assert.NotNull(result);
        Assert.Equal(0, result.Created);
        Assert.Equal(1, result.Skipped);
    }
}
