using Microsoft.EntityFrameworkCore;
using ReferenceManager.Data;
using ReferenceManager.Models;
using ReferenceManager.Requests;

namespace ReferenceManager.Endpoints;

public static class CollectionEndpoints
{
    public static void MapCollectionEndpoints(this WebApplication app)
    {
        // &begin[GetCollection]
        app.MapGet("/collections", async (AppDbContext db) =>
            await db.Collections.ToListAsync())
            .WithName("ListCollections")
            .WithOpenApi();

        app.MapGet("/collections/{id:int}", async (int id, AppDbContext db) =>
            await db.Collections.AsNoTracking().Include(c => c.Papers).FirstOrDefaultAsync(c => c.Id == id) is Collection col
                ? Results.Ok(col)
                : Results.NotFound())
            .WithName("GetCollection")
            .WithOpenApi();
        // &end[GetCollection]

        // &begin[CreateCollection]
        app.MapPost("/collections", async (CollectionRequest req, AppDbContext db) =>
        {
            var col = new Collection { Name = req.Name, Description = req.Description };
            db.Collections.Add(col);
            await db.SaveChangesAsync();
            return Results.Created($"/collections/{col.Id}", col);
        })
        .WithName("CreateCollection")
        .WithOpenApi();
        // &end[CreateCollection]

        // &begin[UpdateCollection]
        app.MapPut("/collections/{id:int}", async (int id, CollectionRequest req, AppDbContext db) =>
        {
            var col = await db.Collections.FindAsync(id);
            if (col is null) return Results.NotFound();

            col.Name = req.Name;
            col.Description = req.Description;
            await db.SaveChangesAsync();
            return Results.Ok(col);
        })
        .WithName("UpdateCollection")
        .WithOpenApi();
        // &end[UpdateCollection]

        // &begin[DeleteCollection]
        app.MapDelete("/collections/{id:int}", async (int id, AppDbContext db) =>
        {
            var col = await db.Collections.FindAsync(id);
            if (col is null) return Results.NotFound();

            db.Collections.Remove(col);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("DeleteCollection")
        .WithOpenApi();
        // &end[DeleteCollection]

        // &begin[AddPaperToCollection]
        app.MapPost("/collections/{id:int}/papers/{paperId:int}", async (int id, int paperId, AppDbContext db) =>
        {
            var col = await db.Collections.Include(c => c.Papers).FirstOrDefaultAsync(c => c.Id == id);
            if (col is null) return Results.NotFound();

            var paper = await db.Papers.FindAsync(paperId);
            if (paper is null) return Results.NotFound();

            if (col.Papers.Any(p => p.Id == paperId)) return Results.Conflict();

            col.Papers.Add(paper);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("AddPaperToCollection")
        .WithOpenApi();
        // &end[AddPaperToCollection]

        // &begin[RemovePaperFromCollection]
        app.MapDelete("/collections/{id:int}/papers/{paperId:int}", async (int id, int paperId, AppDbContext db) =>
        {
            var col = await db.Collections.Include(c => c.Papers).FirstOrDefaultAsync(c => c.Id == id);
            if (col is null) return Results.NotFound();

            var paper = col.Papers.FirstOrDefault(p => p.Id == paperId);
            if (paper is null) return Results.NotFound();

            col.Papers.Remove(paper);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("RemovePaperFromCollection")
        .WithOpenApi();
        // &end[RemovePaperFromCollection]
    }
}
