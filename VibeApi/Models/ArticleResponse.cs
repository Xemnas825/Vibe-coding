namespace VibeApi.Models;

public class ArticleResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Author? Author { get; set; }
    public Category? Category { get; set; }
    public List<Tag> Tags { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsPublished { get; set; }
    public int ViewCount { get; set; }
}