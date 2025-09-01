using memorial_cidade_backend.Models;

namespace memorial_cidade_backend.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<IEnumerable<Photo>> GetAllAsync();
        Task<Photo> GetByIdAsync(int id);
        Task<Photo> CreateAsync(Photo photo);
        Task<Photo> UpdateAsync(int id, Photo photo);
        Task DeleteAsync(int id);
        Task<IEnumerable<Photo>> GetByLocationIdAsync(int locationId);
        Task<IEnumerable<Photo>> GetByPhotographerIdAsync(int photographerId);
        Task<IEnumerable<Photo>> GetBySourceIdAsync(int sourceId);
        Task<IEnumerable<Photo>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Photo>> SearchAsync(string searchTerm, int? yearStart, int? yearEnd);
        Task<Photo> AddTagAsync(int photoId, int tagId);
        Task<Photo> RemoveTagAsync(int photoId, int tagId);
    }
}