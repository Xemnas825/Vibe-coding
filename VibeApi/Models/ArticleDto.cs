namespace VibeApi.Models;

public class ArticleDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public int CategoryId { get; set; }
    public bool IsPublished { get; set; }
    public List<int> TagIds { get; set; } = new();
}