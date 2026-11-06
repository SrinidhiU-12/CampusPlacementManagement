using CampusPlacementManagement.DTOs.PlacementOfficers;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IPlacementOfficerService
    {
        Task<PlacementOfficerResponseDto> RegisterAsync(
            PlacementOfficerRegisterDto dto);

        Task<PlacementOfficerResponseDto?> GetByIdAsync(int id);

        Task<PlacementOfficerResponseDto?> GetByUserIdAsync(int userId);

        Task<IEnumerable<PlacementOfficerResponseDto>> GetAllAsync();

        Task<bool> UpdateAsync(int id, PlacementOfficerUpdateDto dto);

        Task<bool> UpdateStatusAsync(
            int id, PlacementOfficerStatusUpdateDto dto, int adminUserId);
    }
}