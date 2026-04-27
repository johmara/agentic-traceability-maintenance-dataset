using Microsoft.EntityFrameworkCore;
using ReferenceManager.Data;
using ReferenceManager.Models;
using ReferenceManager.Requests;

namespace ReferenceManager.Endpoints;

public static class AuthorEndpoints
{
    public static void MapAuthorEndpoints(this IEndpointRouteBuilder app)
    {
        // &begin[ListAuthors]
        app.MapGet("/authors", async (AppDbContext db) =>
            Results.Ok(await db.Authors.Include(a => a.Papers).ToListAsync()))
        .WithName("ListAuthors")
        .WithOpenApi();
        // &end[ListAuthors]

        // &begin[GetAuthor]
        app.MapGet("/authors/{id:int}", async (int id, AppDbContext db) =>
            await db.Authors.Include(a => a.Papers).FirstOrDefaultAsync(a => a.Id == id) is Author author
                ? Results.Ok(author)
                : Results.NotFound())
        .WithName("GetAuthor")
        .WithOpenApi();
        // &end[GetAuthor]

        // &begin[CreateAuthor]
        app.MapPost("/authors", async (AuthorRequest req, AppDbContext db) =>
        {
            var existing = await db.Authors
                .FirstOrDefaultAsync(a => a.Name.ToLower() == req.Name.ToLower());
            if (existing is not null)
                return Results.Conflict(existing);

            var author = FromRequest(req);
            db.Authors.Add(author);
            await db.SaveChangesAsync();
            return Results.Created($"/api/v1/authors/{author.Id}", author);
        })
        .WithName("CreateAuthor")
        .WithOpenApi();
        // &end[CreateAuthor]

        // &begin[UpdateAuthor]
        app.MapPut("/authors/{id:int}", async (int id, AuthorRequest req, AppDbContext db) =>
        {
            var author = await db.Authors.FindAsync(id);
            if (author is null) return Results.NotFound();

            author.Name = req.Name;
            author.Email = req.Email;
            author.Affiliations = req.Affiliations
                .Select(a => new Affiliation { Name = a.Name, Country = a.Country, City = a.City })
                .ToList();
            await db.SaveChangesAsync();
            return Results.Ok(author);
        })
        .WithName("UpdateAuthor")
        .WithOpenApi();
        // &end[UpdateAuthor]

        // &begin[DeleteAuthor]
        app.MapDelete("/authors/{id:int}", async (int id, AppDbContext db) =>
        {
            var author = await db.Authors.FindAsync(id);
            if (author is null) return Results.NotFound();

            db.Authors.Remove(author);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("DeleteAuthor")
        .WithOpenApi();
        // &end[DeleteAuthor]

        // &begin[MergeAuthors]
        app.MapPost("/authors/{keepId:int}/merge/{mergeId:int}", async (int keepId, int mergeId, AppDbContext db) =>
        {
            if (keepId == mergeId) return Results.BadRequest("Cannot merge author with itself.");

            var keep = await db.Authors.Include(a => a.Papers).FirstOrDefaultAsync(a => a.Id == keepId);
            var merge = await db.Authors.Include(a => a.Papers).FirstOrDefaultAsync(a => a.Id == mergeId);
            if (keep is null || merge is null) return Results.NotFound();

            foreach (var paper in merge.Papers)
            {
                if (!keep.Papers.Any(p => p.Id == paper.Id))
                    keep.Papers.Add(paper);
            }

            foreach (var aff in merge.Affiliations)
            {
                if (!keep.Affiliations.Any(a => a.Name.Equals(aff.Name, StringComparison.OrdinalIgnoreCase)))
                    keep.Affiliations.Add(new Affiliation { Name = aff.Name, Country = aff.Country, City = aff.City });
            }

            db.Authors.Remove(merge);
            await db.SaveChangesAsync();
            return Results.Ok(keep);
        })
        .WithName("MergeAuthors")
        .WithOpenApi();
        // &end[MergeAuthors]
    }

    // &begin[Authors]
    internal static Author FromRequest(AuthorRequest r) => new()
    {
        Name = r.Name,
        Email = r.Email,
        Affiliations = r.Affiliations
            .Select(a => new Affiliation { Name = a.Name, Country = a.Country, City = a.City })
            .ToList()
    };

    internal static async Task<Author> GetOrCreateAsync(
        string name, string? email, List<AffiliationRequest> affiliations,
        AppDbContext db, Dictionary<string, Author> localCache)
    {
        var key = name.Trim().ToLowerInvariant();
        if (localCache.TryGetValue(key, out var cached))
            return cached;

        var existing = await db.Authors.FirstOrDefaultAsync(a => a.Name.ToLower() == key);
        if (existing is not null)
        {
            localCache[key] = existing;
            return existing;
        }

        var author = new Author
        {
            Name = name.Trim(),
            Email = email,
            Affiliations = affiliations
                .Select(a => new Affiliation { Name = a.Name, Country = a.Country, City = a.City })
                .ToList()
        };
        db.Authors.Add(author);
        localCache[key] = author;
        return author;
    }
    // &end[Authors]
}
