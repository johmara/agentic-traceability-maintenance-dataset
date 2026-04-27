namespace ReferenceManager.Requests;

// &begin[Authors]
record AffiliationRequest(string Name, string Country, string City);
record AuthorRequest(string Name, string? Email, List<AffiliationRequest> Affiliations);
// &end[Authors]
