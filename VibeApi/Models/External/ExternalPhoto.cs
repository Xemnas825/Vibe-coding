namespace VibeApi.Models.External;

public class ExternalPhoto
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
}