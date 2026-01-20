namespace VibeApi.Models.External;

public class ExternalAlbum
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
}