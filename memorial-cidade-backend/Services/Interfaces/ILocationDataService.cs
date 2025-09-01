using memorial_cidade_backend.Models;

namespace memorial_cidade_backend.Services.Interfaces
{
    public interface ILocationDataService
    {
        Task<IEnumerable<LocationData>> GetAllAsync();
        Task<LocationData> GetByIdAsync(int id);
        Task<LocationData> CreateAsync(LocationData locationData);
        Task<LocationData> UpdateAsync(int id, LocationData locationData);
        Task DeleteAsync(int id);
        Task<IEnumerable<LocationData>> GetByCoordinatesRangeAsync(double minLat, double maxLat, double minLong, double maxLong);
    }
}