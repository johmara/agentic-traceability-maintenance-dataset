namespace ReferenceManager.Requests;

// &begin[Papers]
record PaperRequest(string Title, List<AuthorRequest> Authors, int Year, string? Abstract, string? Doi, string? Journal, string? Booktitle);
// &end[Papers]
