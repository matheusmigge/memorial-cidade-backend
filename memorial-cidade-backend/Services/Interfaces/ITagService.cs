using memorial_cidade_backend.Models;

namespace memorial_cidade_backend.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag> GetByIdAsync(int id);
        Task<Tag> CreateAsync(Tag tag);
        Task<Tag> UpdateAsync(int id, Tag tag);
        Task DeleteAsync(int id);
        Task<IEnumerable<Tag>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Tag>> SearchByNameAsync(string searchTerm);
        Task<IEnumerable<Photo>> GetTagPhotosAsync(int tagId);
    }
}