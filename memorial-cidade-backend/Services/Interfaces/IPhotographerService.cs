using memorial_cidade_backend.Models;

namespace memorial_cidade_backend.Services.Interfaces
{
    public interface IPhotographerService
    {
        Task<IEnumerable<Photographer>> GetAllAsync();
        Task<Photographer> GetByIdAsync(int id);
        Task<Photographer> CreateAsync(Photographer photographer);
        Task<Photographer> UpdateAsync(int id, Photographer photographer);
        Task DeleteAsync(int id);
        Task<IEnumerable<Photographer>> SearchByNameAsync(string searchTerm);
        Task<IEnumerable<Photo>> GetPhotographerPhotosAsync(int photographerId);
    }
}