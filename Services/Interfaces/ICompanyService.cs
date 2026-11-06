using CampusPlacementManagement.DTOs.Companies;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyResponseDto> RegisterAsync(CompanyRegisterDto dto);
        Task<CompanyResponseDto?> GetByIdAsync(int id);
        Task<CompanyResponseDto?> GetByUserIdAsync(int userId);
        Task<IEnumerable<CompanyResponseDto>> GetAllAsync();
        Task<bool> UpdateAsync(int id, CompanyUpdateDto dto);
        Task<bool> UpdateStatusAsync(int id, string status, string? rejectionReason, int adminUserId);
    }
}