namespace BugStore.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public decimal Price { get; set; }
}