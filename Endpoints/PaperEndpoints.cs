using Microsoft.EntityFrameworkCore;
using ReferenceManager.Data;
using ReferenceManager.Models;
using ReferenceManager.Requests;

namespace ReferenceManager.Endpoints;

public static class PaperEndpoints
{
    public static void MapPaperEndpoints(this WebApplication app)
    {
        // &begin[GetPaper]
        app.MapGet("/papers", async (AppDbContext db) =>
            await db.Papers.ToListAsync())
            .WithName("ListPapers")
            .WithOpenApi();

        app.MapGet("/papers/{id:int}", async (int id, AppDbContext db) =>
            await db.Papers.FindAsync(id) is Paper paper
                ? Results.Ok(paper)
                : Results.NotFound())
            .WithName("GetPaper")
            .WithOpenApi();
        // &end[GetPaper]

        // &begin[CreatePaper]
        app.MapPost("/papers", async (PaperRequest req, AppDbContext db) =>
        {
            var paper = new Paper
            {
                Title = req.Title,
                Authors = req.Authors.Select(ToAuthor).ToList(),
                Year = req.Year,
                Abstract = req.Abstract,
                Doi = req.Doi
            };
            db.Papers.Add(paper);
            await db.SaveChangesAsync();
            return Results.Created($"/papers/{paper.Id}", paper);
        })
        .WithName("CreatePaper")
        .WithOpenApi();
        // &end[CreatePaper]

        // &begin[UpdatePaper]
        app.MapPut("/papers/{id:int}", async (int id, PaperRequest req, AppDbContext db) =>
        {
            var paper = await db.Papers.FindAsync(id);
            if (paper is null) return Results.NotFound();

            paper.Title = req.Title;
            paper.Authors = req.Authors.Select(ToAuthor).ToList();
            paper.Year = req.Year;
            paper.Abstract = req.Abstract;
            paper.Doi = req.Doi;
            await db.SaveChangesAsync();
            return Results.Ok(paper);
        })
        .WithName("UpdatePaper")
        .WithOpenApi();
        // &end[UpdatePaper]

        // &begin[DeletePaper]
        app.MapDelete("/papers/{id:int}", async (int id, AppDbContext db) =>
        {
            var paper = await db.Papers.FindAsync(id);
            if (paper is null) return Results.NotFound();

            db.Papers.Remove(paper);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("DeletePaper")
        .WithOpenApi();
        // &end[DeletePaper]
    }

    private static Author ToAuthor(AuthorRequest r) => new()
    {
        Name = r.Name,
        Email = r.Email,
        Affiliations = r.Affiliations
            .Select(a => new Affiliation { Name = a.Name, Country = a.Country, City = a.City })
            .ToList()
    };
}
