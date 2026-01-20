namespace VibeApi.Models;

public class ArticleTag
{
    public int ArticleId { get; set; }
    public int TagId { get; set; }
    public DateTime AssignedAt { get; set; }
}