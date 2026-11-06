
using CampusPlacementManagement.DTOs.PlacementOfficers;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services.Implementations
{
    public class PlacementOfficerService : IPlacementOfficerService
    {
        private readonly IPlacementOfficerRepository _repository;
        private readonly PasswordService _passwordService;

        public PlacementOfficerService(
            IPlacementOfficerRepository repository,
            PasswordService passwordService)
        {
            _repository = repository;
            _passwordService = passwordService;
        }

        public async Task<PlacementOfficerResponseDto> RegisterAsync(
            PlacementOfficerRegisterDto dto)
        {
            if (await _repository.EmailExistsAsync(dto.Email))
                throw new Exception("Email already registered.");

            if (!await _repository.CollegeExistsAndApprovedAsync(dto.CollegeId))
                throw new Exception("College does not exist or is not approved.");

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email.Trim().ToLowerInvariant(),
                PasswordHash = _passwordService.HashPassword(dto.Password),
                Role = "PlacementOfficer",
                IsActive = true
            };

            var officer = new PlacementOfficer
            {
                ApplicationUser = user,
                CollegeId = dto.CollegeId,
                PhoneNumber = dto.PhoneNumber,
                Designation = dto.Designation,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(officer);
            await _repository.SaveChangesAsync();

            var savedOfficer = await _repository.GetByIdAsync(
    officer.PlacementOfficerId);

            if (savedOfficer == null)
                throw new Exception("Placement Officer registration failed.");

            return MapToDto(savedOfficer);

        }

        public async Task<PlacementOfficerResponseDto?> GetByIdAsync(int id)
        {
            var officer = await _repository.GetByIdAsync(id);
            return officer == null ? null : MapToDto(officer);
        }

        public async Task<PlacementOfficerResponseDto?> GetByUserIdAsync(int userId)
        {
            var officer = await _repository.GetByUserIdAsync(userId);
            return officer == null ? null : MapToDto(officer);
        }

        public async Task<IEnumerable<PlacementOfficerResponseDto>> GetAllAsync()
        {
            var officers = await _repository.GetAllAsync();
            return officers.Select(MapToDto);
        }

        public async Task<bool> UpdateAsync(int id, PlacementOfficerUpdateDto dto)
        {
            var officer = await _repository.GetByIdAsync(id);

            if (officer == null)
                return false;

            officer.ApplicationUser.FullName = dto.FullName;
            officer.PhoneNumber = dto.PhoneNumber;
            officer.Designation = dto.Designation;
            officer.UpdatedAt = DateTime.UtcNow;

            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusAsync(
            int id,
            PlacementOfficerStatusUpdateDto dto,
            int adminUserId)
        {
            var officer = await _repository.GetByIdAsync(id);

            if (officer == null)
                return false;

            if (dto.Status != "Approved" && dto.Status != "Rejected")
                throw new Exception("Invalid status.");

            if (dto.Status == "Rejected" &&
                string.IsNullOrWhiteSpace(dto.RejectionReason))
                throw new Exception("Rejection reason is required.");

            officer.Status = dto.Status;
            officer.RejectionReason =
                dto.Status == "Rejected" ? dto.RejectionReason : null;

            officer.ReviewedByUserId = adminUserId;
            officer.ReviewedAt = DateTime.UtcNow;
            officer.ApplicationUser.IsActive = dto.Status == "Approved";

            await _repository.SaveChangesAsync();
            return true;
        }

        private static PlacementOfficerResponseDto MapToDto(
            PlacementOfficer officer)
        {
            return new PlacementOfficerResponseDto
            {
                PlacementOfficerId = officer.PlacementOfficerId,
                UserId = officer.UserId,
                FullName = officer.ApplicationUser.FullName,
                Email = officer.ApplicationUser.Email,
                CollegeId = officer.CollegeId,
                CollegeName = officer.College.CollegeName,
                PhoneNumber = officer.PhoneNumber,
                Designation = officer.Designation,
                Status = officer.Status,
                RejectionReason = officer.RejectionReason,
                CreatedAt = officer.CreatedAt,
                ReviewedAt = officer.ReviewedAt
            };
        }
    }
}
