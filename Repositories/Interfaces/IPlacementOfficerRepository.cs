
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IPlacementOfficerRepository
    {
        // Registration
        Task<bool> EmailExistsAsync(string email);

        Task<bool> CollegeExistsAndApprovedAsync(int collegeId);

        Task AddAsync(PlacementOfficer placementOfficer);

        // Retrieval
        Task<PlacementOfficer?> GetByIdAsync(int id);

        Task<PlacementOfficer?> GetByUserIdAsync(int userId);

        Task<IEnumerable<PlacementOfficer>> GetAllAsync();

        // Updating
        Task UpdateAsync(PlacementOfficer placementOfficer);

        Task SaveChangesAsync();
    }
}
