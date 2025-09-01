using memorial_cidade_backend.Data;
using memorial_cidade_backend.Models;
using memorial_cidade_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace memorial_cidade_backend.Services
{
    public class TagService : ITagService
    {
        private readonly AppDbContext _context;

        public TagService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync() 
        {
            return await _context.Tags
                .Include(t => t.TagCategory)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            var tag = await _context.Tags
                .Include(t => t.TagCategory)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null)
                throw new KeyNotFoundException($"Tag with ID {id} not found.");

            return tag;
        }

        public async Task<Tag> CreateAsync(Tag tag)
        {
            var categoryExists = await _context.TagCategories
                .AnyAsync(tc => tc.Id == tag.TagCategoryId);

            if (!categoryExists)
                throw new KeyNotFoundException($"TagCategory with ID {tag.TagCategoryId} not found.");

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag> UpdateAsync(int id, Tag tag)
        {
            var existingTag = await _context.Tags.FindAsync(id);
            if (existingTag == null)
                throw new KeyNotFoundException($"Tag with ID {id} not found.");

            var categoryExists = await _context.TagCategories
                .AnyAsync(tc => tc.Id == tag.TagCategoryId);

            if (!categoryExists)
                throw new KeyNotFoundException($"TagCategory with ID {tag.TagCategoryId} not found.");

            existingTag.Name = tag.Name;
            existingTag.IconUrl = tag.IconUrl;
            existingTag.TagCategoryId = tag.TagCategoryId;

            await _context.SaveChangesAsync();
            return existingTag;
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await _context.Tags
                .Include(t => t.TagCategory)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null)
                throw new KeyNotFoundException($"Tag with ID {id} not found.");

            // Check if tag is associated with any photos
            var hasPhotos = await _context.Photos
                .AnyAsync(p => p.Tags.Any(t => t.Id == id));

            if (hasPhotos)
                throw new InvalidOperationException("Cannot delete tag with associated photos.");

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tag>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.Tags
                .Include(t => t.TagCategory)
                .Where(t => t.TagCategoryId == categoryId)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tag>> SearchByNameAsync(string searchTerm)
        {
            return await _context.Tags
                .Include(t => t.TagCategory)
                .Where(t => t.Name.Contains(searchTerm))
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Photo>> GetTagPhotosAsync(int tagId)
        {
            var tag = await _context.Tags
                .Include(t => t.TagCategory)
                .FirstOrDefaultAsync(t => t.Id == tagId);

            if (tag == null)
                throw new KeyNotFoundException($"Tag with ID {tagId} not found.");

            var photos = await _context.Photos
                .Include(p => p.Tags)
                .Where(p => p.Tags.Any(t => t.Id == tagId))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return photos;
        }
    }
}