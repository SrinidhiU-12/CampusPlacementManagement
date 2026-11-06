using CampusPlacementManagement.DTOs.Colleges;

namespace CampusPlacementManagement.Services.Interfaces;

public interface ICollegeService
{
    Task<CollegeResponseDto> RegisterAsync(CollegeRegistrationDto dto);
    Task<List<CollegeResponseDto>> GetAllAsync();
    Task<CollegeResponseDto?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, CollegeUpdateDto dto);
    Task<bool> UpdateStatusAsync(
        int id, CollegeStatusUpdateDto dto, int adminUserId);
}