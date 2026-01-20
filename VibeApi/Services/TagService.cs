using VibeApi.Models;

namespace VibeApi.Services;

public interface ITagService
{
    Task<IEnumerable<Tag>> GetAllTagsAsync();
    Task<Tag?> GetTagByIdAsync(int id);
    Task<Tag> CreateTagAsync(TagDto tagDto);
    Task<bool> UpdateTagAsync(int id, TagDto tagDto);
    Task<bool> DeleteTagAsync(int id);
}

public class TagService : ITagService
{
    private static readonly List<Tag> _tags = new();
    private static int _nextId = 1;

    public Task<IEnumerable<Tag>> GetAllTagsAsync()
    {
        return Task.FromResult(_tags.AsEnumerable());
    }

    public Task<Tag?> GetTagByIdAsync(int id)
    {
        var tag = _tags.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(tag);
    }

    public Task<Tag> CreateTagAsync(TagDto tagDto)
    {
        var tag = new Tag
        {
            Id = _nextId++,
            Name = tagDto.Name,
            Color = tagDto.Color,
            CreatedAt = DateTime.UtcNow
        };

        _tags.Add(tag);
        return Task.FromResult(tag);
    }

    public Task<bool> UpdateTagAsync(int id, TagDto tagDto)
    {
        var tag = _tags.FirstOrDefault(t => t.Id == id);
        if (tag == null) return Task.FromResult(false);

        tag.Name = tagDto.Name;
        tag.Color = tagDto.Color;

        return Task.FromResult(true);
    }

    public Task<bool> DeleteTagAsync(int id)
    {
        var tag = _tags.FirstOrDefault(t => t.Id == id);
        if (tag == null) return Task.FromResult(false);

        _tags.Remove(tag);
        return Task.FromResult(true);
    }
}