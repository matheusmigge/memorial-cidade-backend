using memorial_cidade_backend.Data;
using memorial_cidade_backend.Models;
using memorial_cidade_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace memorial_cidade_backend.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly AppDbContext _context;

        public OrganizationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Organization>> GetAllAsync()
        {
            return await _context.Organizations
                .OrderBy(o => o.Name)
                .ToListAsync();
        }

        public async Task<Organization> GetByIdAsync(int id)
        {
            var organization = await _context.Organizations
                .FirstOrDefaultAsync(o => o.Id == id);

            if (organization == null)
                throw new KeyNotFoundException($"Organization with ID {id} not found.");

            return organization;
        }

        public async Task<Organization> CreateAsync(Organization organization)
        {
            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();
            return organization;
        }

        public async Task<Organization> UpdateAsync(int id, Organization organization)
        {
            var existingOrganization = await _context.Organizations.FindAsync(id);
            if (existingOrganization == null)
                throw new KeyNotFoundException($"Organization with ID {id} not found.");

            existingOrganization.Name = organization.Name;
            existingOrganization.Description = organization.Description;
            existingOrganization.LogoUrl = organization.LogoUrl;
            existingOrganization.WebsiteUrl = organization.WebsiteUrl;

            await _context.SaveChangesAsync();
            return existingOrganization;
        }

        public async Task DeleteAsync(int id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
                throw new KeyNotFoundException($"Organization with ID {id} not found.");

            _context.Organizations.Remove(organization);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Organization>> SearchByNameAsync(string searchTerm)
        {
            return await _context.Organizations
                .Where(o => o.Name.Contains(searchTerm))
                .OrderBy(o => o.Name)
                .ToListAsync();
        }
    }
}