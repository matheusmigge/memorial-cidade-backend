using memorial_cidade_backend.Models;

namespace memorial_cidade_backend.Services.Interfaces
{
    public interface IOrganizationService
    {
        Task<IEnumerable<Organization>> GetAllAsync();
        Task<Organization> GetByIdAsync(int id);
        Task<Organization> CreateAsync(Organization organization);
        Task<Organization> UpdateAsync(int id, Organization organization);
        Task DeleteAsync(int id);
        Task<IEnumerable<Organization>> SearchByNameAsync(string searchTerm);
    }
}