namespace ReferenceManager.Responses;

public record SearchResultItem(PaperResponse Paper, double Score);
public record SearchResponse(List<SearchResultItem> Items, int Total);
