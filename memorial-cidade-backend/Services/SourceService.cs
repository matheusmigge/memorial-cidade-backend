using memorial_cidade_backend.Data;
using memorial_cidade_backend.Models;
using memorial_cidade_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace memorial_cidade_backend.Services
{
    public class SourceService : ISourceService
    {
        private readonly AppDbContext _context;

        public SourceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Source>> GetAllAsync()
        {
            return await _context.Sources
                .Include(s => s.Organization)
                .OrderBy(s => s.Collection)
                .ToListAsync();
        }

        public async Task<Source> GetByIdAsync(int id)
        {
            var source = await _context.Sources
                .Include(s => s.Organization)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (source == null)
                throw new KeyNotFoundException($"Source with ID {id} not found.");

            return source;
        }

        public async Task<Source> CreateAsync(Source source)
        {
            if (source.OrganizationId.HasValue)
            {
                var organizationExists = await _context.Organizations
                    .AnyAsync(o => o.Id == source.OrganizationId);
                
                if (!organizationExists)
                    throw new KeyNotFoundException($"Organization with ID {source.OrganizationId} not found.");
            }

            _context.Sources.Add(source);
            await _context.SaveChangesAsync();
            return source;
        }

        public async Task<Source> UpdateAsync(int id, Source source)
        {
            var existingSource = await _context.Sources.FindAsync(id);
            if (existingSource == null)
                throw new KeyNotFoundException($"Source with ID {id} not found.");

            if (source.OrganizationId.HasValue)
            {
                var organizationExists = await _context.Organizations
                    .AnyAsync(o => o.Id == source.OrganizationId);
                
                if (!organizationExists)
                    throw new KeyNotFoundException($"Organization with ID {source.OrganizationId} not found.");
            }

            existingSource.DocumentPhotoUrl = source.DocumentPhotoUrl;
            existingSource.Collection = source.Collection;
            existingSource.SourceUrl = source.SourceUrl;
            existingSource.OrganizationId = source.OrganizationId;

            await _context.SaveChangesAsync();
            return existingSource;
        }

        public async Task DeleteAsync(int id)
        {
            var source = await _context.Sources
                .Include(s => s.Organization)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (source == null)
                throw new KeyNotFoundException($"Source with ID {id} not found.");

            var hasPhotos = await _context.Photos.AnyAsync(p => p.SourceId == id);
            if (hasPhotos)
                throw new InvalidOperationException("Cannot delete source with associated photos.");

            _context.Sources.Remove(source);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Source>> GetByOrganizationIdAsync(int organizationId)
        {
            return await _context.Sources
                .Include(s => s.Organization)
                .Where(s => s.OrganizationId == organizationId)
                .OrderBy(s => s.Collection)
                .ToListAsync();
        }

        public async Task<IEnumerable<Source>> SearchByCollectionAsync(string collection)
        {
            return await _context.Sources
                .Include(s => s.Organization)
                .Where(s => s.Collection != null && s.Collection.Contains(collection))
                .OrderBy(s => s.Collection)
                .ToListAsync();
        }
    }
}