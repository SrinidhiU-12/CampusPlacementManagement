using CampusPlacementManagement.DTOs.Auth;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}