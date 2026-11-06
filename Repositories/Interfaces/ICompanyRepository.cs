using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task AddAsync(Company company);
        Task<Company?> GetByIdAsync(int id);
        Task<Company?> GetByUserIdAsync(int userId);
        Task<IEnumerable<Company>> GetAllAsync();
        Task UpdateAsync(Company company);
        Task SaveChangesAsync();
    }
}