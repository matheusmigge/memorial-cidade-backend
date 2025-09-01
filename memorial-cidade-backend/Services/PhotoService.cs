using memorial_cidade_backend.Data;
using memorial_cidade_backend.Models;
using memorial_cidade_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace memorial_cidade_backend.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly AppDbContext _context;

        public PhotoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Photo>> GetAllAsync()
        {
            return await _context.Photos
                .Include(p => p.Location)
                .Include(p => p.Photographer)
                .Include(p => p.Source)
                .Include(p => p.Tags)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Photo> GetByIdAsync(int id)
        {
            var photo = await _context.Photos
                .Include(p => p.Location)
                .Include(p => p.Photographer)
                .Include(p => p.Source)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (photo == null)
                throw new KeyNotFoundException($"Photo with ID {id} not found.");

            return photo;
        }

        public async Task<Photo> CreateAsync(Photo photo)
        {
            photo.CreatedAt = DateTime.Now;
            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();
            return photo;
        }

        public async Task<Photo> UpdateAsync(int id, Photo photo)
        {
            var existingPhoto = await _context.Photos
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingPhoto == null)
                throw new KeyNotFoundException($"Photo with ID {id} not found.");

            existingPhoto.Url = photo.Url;
            existingPhoto.Title = photo.Title;
            existingPhoto.YearStart = photo.YearStart;
            existingPhoto.YearEnd = photo.YearEnd;
            existingPhoto.UserNote = photo.UserNote;
            existingPhoto.PhotographerId = photo.PhotographerId;
            existingPhoto.LocationId = photo.LocationId;
            existingPhoto.SourceId = photo.SourceId;
            existingPhoto.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingPhoto;
        }

        public async Task DeleteAsync(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
                throw new KeyNotFoundException($"Photo with ID {id} not found.");

            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Photo>> GetByLocationIdAsync(int locationId)
        {
            return await _context.Photos
                .Include(p => p.Tags)
                .Where(p => p.LocationId == locationId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Photo>> GetByPhotographerIdAsync(int photographerId)
        {
            return await _context.Photos
                .Include(p => p.Tags)
                .Where(p => p.PhotographerId == photographerId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Photo>> GetBySourceIdAsync(int sourceId)
        {
            return await _context.Photos
                .Include(p => p.Tags)
                .Where(p => p.SourceId == sourceId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Photo>> GetByUserIdAsync(int userId)
        {
            return await _context.Photos
                .Include(p => p.Tags)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Photo>> SearchAsync(string searchTerm, int? yearStart, int? yearEnd)
        {
            var query = _context.Photos
                .Include(p => p.Tags)
                .Include(p => p.Location)
                .Include(p => p.Photographer)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Title.Contains(searchTerm) || 
                                       p.UserNote.Contains(searchTerm));
            }

            if (yearStart.HasValue)
            {
                query = query.Where(p => p.YearStart >= yearStart.Value);
            }

            if (yearEnd.HasValue)
            {
                query = query.Where(p => p.YearEnd <= yearEnd.Value || p.YearStart <= yearEnd.Value);
            }

            return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task<Photo> AddTagAsync(int photoId, int tagId)
        {
            var photo = await _context.Photos
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == photoId);

            if (photo == null)
                throw new KeyNotFoundException($"Photo with ID {photoId} not found.");

            var tag = await _context.Tags.FindAsync(tagId);
            if (tag == null)
                throw new KeyNotFoundException($"Tag with ID {tagId} not found.");

            photo.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return photo;
        }

        public async Task<Photo> RemoveTagAsync(int photoId, int tagId)
        {
            var photo = await _context.Photos
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == photoId);

            if (photo == null)
                throw new KeyNotFoundException($"Photo with ID {photoId} not found.");

            var tag = photo.Tags.FirstOrDefault(t => t.Id == tagId);
            if (tag == null)
                throw new KeyNotFoundException($"Tag with ID {tagId} not found in this photo.");

            photo.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return photo;
        }
    }
}