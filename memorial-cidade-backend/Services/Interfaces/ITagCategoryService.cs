using memorial_cidade_backend.Models;

namespace memorial_cidade_backend.Services.Interfaces
{
    public interface ITagCategoryService
    {
        Task<IEnumerable<TagCategory>> GetAllAsync();
        Task<TagCategory> GetByIdAsync(int id);
        Task<TagCategory> CreateAsync(TagCategory tagCategory);
        Task<TagCategory> UpdateAsync(int id, TagCategory tagCategory);
        Task DeleteAsync(int id);
        Task<IEnumerable<TagCategory>> SearchByNameAsync(string searchTerm);
        Task<IEnumerable<Tag>> GetCategoryTagsAsync(int categoryId);
    }
}