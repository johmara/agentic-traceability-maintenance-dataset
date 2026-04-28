namespace ReferenceManager.Models;

// &begin[Authors]
public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public List<Affiliation> Affiliations { get; set; } = [];
    public List<Paper> Papers { get; set; } = [];
}
// &end[Authors]
