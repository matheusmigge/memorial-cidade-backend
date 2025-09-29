using memorial_cidade_backend.Data;
using memorial_cidade_backend.Models;
using memorial_cidade_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace memorial_cidade_backend.Services
{
    public class LocationDataService : ILocationDataService
    {
        private readonly AppDbContext _context;

        public LocationDataService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LocationData>> GetAllAsync()
        {
            return await _context.LocationDatas.ToListAsync();
        }

        public async Task<LocationData> GetByIdAsync(int id)
        {
            var location = await _context.LocationDatas
                .Include(l => l.Photo)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (location == null)
                throw new KeyNotFoundException($"Location with ID {id} not found.");

            return location;
        }

        public async Task<LocationData> CreateAsync(LocationData locationData)
        {
            _context.LocationDatas.Add(locationData);
            await _context.SaveChangesAsync();
            return locationData;
        }

        public async Task<LocationData> UpdateAsync(int id, LocationData locationData)
        {
            var existingLocation = await _context.LocationDatas.FindAsync(id);
            if (existingLocation == null)
                throw new KeyNotFoundException($"Location with ID {id} not found.");

            existingLocation.Latitude = locationData.Latitude;
            existingLocation.Longitude = locationData.Longitude;
            existingLocation.Heading = locationData.Heading;
            existingLocation.GoogleEarthPhotoUrl = locationData.GoogleEarthPhotoUrl;
            existingLocation.GoogleEarthUrl = locationData.GoogleEarthUrl;
            existingLocation.GoogleStreetViewEmbedUrl = locationData.GoogleStreetViewEmbedUrl;

            await _context.SaveChangesAsync();
            return existingLocation;
        }

        public async Task DeleteAsync(int id)
        {
            var location = await _context.LocationDatas.FindAsync(id);
            if (location == null)
                throw new KeyNotFoundException($"Location with ID {id} not found.");

            _context.LocationDatas.Remove(location);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LocationData>> GetByCoordinatesRangeAsync(double minLat, double maxLat,
            double minLong, double maxLong)
        {
            return await _context.LocationDatas
                .Where(l => l.Latitude >= minLat && l.Latitude <= maxLat &&
                            l.Longitude >= minLong && l.Longitude <= maxLong)
                .ToListAsync();
        }
    }
}