namespace VibeApi.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#000000";
    public DateTime CreatedAt { get; set; }
}

public class TagDto
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#000000";
}