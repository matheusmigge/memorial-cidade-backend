using memorial_cidade_backend.Models;

namespace memorial_cidade_backend.Services.Interfaces
{
    public interface ISourceService
    {
        Task<IEnumerable<Source>> GetAllAsync();
        Task<Source> GetByIdAsync(int id);
        Task<Source> CreateAsync(Source source);
        Task<Source> UpdateAsync(int id, Source source);
        Task DeleteAsync(int id);
        Task<IEnumerable<Source>> GetByOrganizationIdAsync(int organizationId);
        Task<IEnumerable<Source>> SearchByCollectionAsync(string collection);
    }
}