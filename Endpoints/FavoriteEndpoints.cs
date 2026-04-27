using Microsoft.EntityFrameworkCore;
using ReferenceManager.Data;
using ReferenceManager.Responses;

namespace ReferenceManager.Endpoints;

public static class FavoriteEndpoints
{
    public static void MapFavoriteEndpoints(this IEndpointRouteBuilder app)
    {
        // &begin[ListFavorites]
        app.MapGet("/favorites", async (AppDbContext db, int limit = 25, int offset = 0) =>
        {
            var query = db.Papers.Include(p => p.Authors).Include(p => p.Groups).AsNoTracking().Where(p => p.IsFavorited);
            var total = await query.CountAsync();
            var items = await query.Skip(offset).Take(limit).ToListAsync();
            return Results.Ok(new PagedResult<PaperResponse>(items.Select(p => p.ToResponse()).ToList(), total, limit, offset));
        })
        .WithName("ListFavorites")
        .WithOpenApi();
        // &end[ListFavorites]

        // &begin[ToggleFavorite]
        app.MapPut("/papers/{id:int}/favorite", async (int id, AppDbContext db) =>
        {
            var paper = await db.Papers.FindAsync(id);
            if (paper is null) return Results.NotFound();

            paper.IsFavorited = !paper.IsFavorited;
            await db.SaveChangesAsync();
            return Results.Ok(new { paper.Id, paper.IsFavorited });
        })
        .WithName("ToggleFavorite")
        .WithOpenApi();
        // &end[ToggleFavorite]
    }
}
