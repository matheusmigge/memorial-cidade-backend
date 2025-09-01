using memorial_cidade_backend.Data;
using memorial_cidade_backend.Models;
using memorial_cidade_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace memorial_cidade_backend.Services
{
    public class PhotographerService : IPhotographerService
    {
        private readonly AppDbContext _context;

        public PhotographerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Photographer>> GetAllAsync()
        {
            return await _context.Photographers
                .Include(p => p.Photos)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Photographer> GetByIdAsync(int id)
        {
            var photographer = await _context.Photographers
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (photographer == null)
                throw new KeyNotFoundException($"Photographer with ID {id} not found.");

            return photographer;
        }

        public async Task<Photographer> CreateAsync(Photographer photographer)
        {
            _context.Photographers.Add(photographer);
            await _context.SaveChangesAsync();
            return photographer;
        }

        public async Task<Photographer> UpdateAsync(int id, Photographer photographer)
        {
            var existingPhotographer = await _context.Photographers.FindAsync(id);
            if (existingPhotographer == null)
                throw new KeyNotFoundException($"Photographer with ID {id} not found.");

            existingPhotographer.Name = photographer.Name;
            existingPhotographer.Biography = photographer.Biography;
            existingPhotographer.BirthDate = photographer.BirthDate;
            existingPhotographer.DeathDate = photographer.DeathDate;

            await _context.SaveChangesAsync();
            return existingPhotographer;
        }

        public async Task DeleteAsync(int id)
        {
            var photographer = await _context.Photographers
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (photographer == null)
                throw new KeyNotFoundException($"Photographer with ID {id} not found.");

            if (photographer.Photos.Any())
                throw new InvalidOperationException("Cannot delete photographer with associated photos.");

            _context.Photographers.Remove(photographer);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Photographer>> SearchByNameAsync(string searchTerm)
        {
            return await _context.Photographers
                .Where(p => p.Name.Contains(searchTerm))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Photo>> GetPhotographerPhotosAsync(int photographerId)
        {
            var photographer = await _context.Photographers
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(p => p.Id == photographerId);

            if (photographer == null)
                throw new KeyNotFoundException($"Photographer with ID {photographerId} not found.");

            return photographer.Photos.OrderByDescending(p => p.CreatedAt);
        }
    }
}