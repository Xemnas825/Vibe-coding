using VibeApi.Models;

namespace VibeApi.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<Category> CreateCategoryAsync(CategoryDto categoryDto);
    Task<bool> UpdateCategoryAsync(int id, CategoryDto categoryDto);
    Task<bool> DeleteCategoryAsync(int id);
}

public class CategoryService : ICategoryService
{
    private static readonly List<Category> _categories = new();
    private static int _nextId = 1;

    public Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return Task.FromResult(_categories.AsEnumerable());
    }

    public Task<Category?> GetCategoryByIdAsync(int id)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(category);
    }

    public Task<Category> CreateCategoryAsync(CategoryDto categoryDto)
    {
        var category = new Category
        {
            Id = _nextId++,
            Name = categoryDto.Name,
            Description = categoryDto.Description,
            IsActive = categoryDto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _categories.Add(category);
        return Task.FromResult(category);
    }

    public Task<bool> UpdateCategoryAsync(int id, CategoryDto categoryDto)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);
        if (category == null) return Task.FromResult(false);

        category.Name = categoryDto.Name;
        category.Description = categoryDto.Description;
        category.IsActive = categoryDto.IsActive;

        return Task.FromResult(true);
    }

    public Task<bool> DeleteCategoryAsync(int id)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);
        if (category == null) return Task.FromResult(false);

        _categories.Remove(category);
        return Task.FromResult(true);
    }
}