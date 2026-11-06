using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces;

public interface ICollegeRepository
{
    Task<bool> ExistsByCodeAsync(string code);
    Task<List<College>> GetAllAsync();
    Task<College?> GetByIdAsync(int id);
    Task AddAsync(College college);
    Task SaveChangesAsync();
}