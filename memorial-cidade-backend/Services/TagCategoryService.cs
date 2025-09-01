using memorial_cidade_backend.Data;
using memorial_cidade_backend.Models;
using memorial_cidade_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace memorial_cidade_backend.Services
{
    public class TagCategoryService : ITagCategoryService
    {
        private readonly AppDbContext _context;

        public TagCategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TagCategory>> GetAllAsync()
        {
            return await _context.TagCategories
                .OrderBy(tc => tc.Name)
                .ToListAsync();
        }

        public async Task<TagCategory> GetByIdAsync(int id)
        {
            var tagCategory = await _context.TagCategories
                .FirstOrDefaultAsync(tc => tc.Id == id);

            if (tagCategory == null)
                throw new KeyNotFoundException($"TagCategory with ID {id} not found.");

            return tagCategory;
        }

        public async Task<TagCategory> CreateAsync(TagCategory tagCategory)
        {
            _context.TagCategories.Add(tagCategory);
            await _context.SaveChangesAsync();
            return tagCategory;
        }

        public async Task<TagCategory> UpdateAsync(int id, TagCategory tagCategory)
        {
            var existingCategory = await _context.TagCategories.FindAsync(id);
            if (existingCategory == null)
                throw new KeyNotFoundException($"TagCategory with ID {id} not found.");

            existingCategory.Name = tagCategory.Name;
            existingCategory.Description = tagCategory.Description;
            existingCategory.IconUrl = tagCategory.IconUrl;

            await _context.SaveChangesAsync();
            return existingCategory;
        }

        public async Task DeleteAsync(int id)
        {
            var tagCategory = await _context.TagCategories.FindAsync(id);
            if (tagCategory == null)
                throw new KeyNotFoundException($"TagCategory with ID {id} not found.");

            var hasTags = await _context.Tags.AnyAsync(t => t.TagCategoryId == id);
            if (hasTags)
                throw new InvalidOperationException("Cannot delete category with associated tags.");

            _context.TagCategories.Remove(tagCategory);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TagCategory>> SearchByNameAsync(string searchTerm)
        {
            return await _context.TagCategories
                .Where(tc => tc.Name.Contains(searchTerm))
                .OrderBy(tc => tc.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tag>> GetCategoryTagsAsync(int categoryId)
        {
            var category = await _context.TagCategories
                .FirstOrDefaultAsync(tc => tc.Id == categoryId);

            if (category == null)
                throw new KeyNotFoundException($"TagCategory with ID {categoryId} not found.");

            return await _context.Tags
                .Where(t => t.TagCategoryId == categoryId)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }
    }
}