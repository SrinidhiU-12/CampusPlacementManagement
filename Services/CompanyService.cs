using CampusPlacementManagement.DTOs.Companies;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;
        private readonly PasswordService _passwordService;

        public CompanyService(
            ICompanyRepository repository,
            PasswordService passwordService)
        {
            _repository = repository;
            _passwordService = passwordService;
        }

        public async Task<CompanyResponseDto> RegisterAsync(CompanyRegisterDto dto)
        {
            if (await _repository.EmailExistsAsync(dto.Email))
                throw new Exception("Email already registered.");

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = _passwordService.HashPassword(dto.Password),
                Role = "Company",
                IsActive = false,
                CreatedAt = DateTime.UtcNow
            };

            var company = new Company
            {
                ApplicationUser = user,
                CompanyName = dto.CompanyName,
                Industry = dto.Industry,
                Website = dto.Website,
                Description = dto.Description,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(company);
            await _repository.SaveChangesAsync();

            var savedCompany = await _repository.GetByIdAsync(company.CompanyId);

            if (savedCompany == null)
                throw new Exception("Company registration failed.");

            return MapToDto(savedCompany);
        }

        public async Task<CompanyResponseDto?> GetByIdAsync(int id)
        {
            var company = await _repository.GetByIdAsync(id);
            return company == null ? null : MapToDto(company);
        }

        public async Task<CompanyResponseDto?> GetByUserIdAsync(int userId)
        {
            var company = await _repository.GetByUserIdAsync(userId);
            return company == null ? null : MapToDto(company);
        }

        public async Task<IEnumerable<CompanyResponseDto>> GetAllAsync()
        {
            var companies = await _repository.GetAllAsync();
            return companies.Select(MapToDto);
        }

        public async Task<bool> UpdateAsync(int id, CompanyUpdateDto dto)
        {
            var company = await _repository.GetByIdAsync(id);

            if (company == null)
                return false;

            company.CompanyName = dto.CompanyName;
            company.Industry = dto.Industry;
            company.Website = dto.Website;
            company.Description = dto.Description;
            company.PhoneNumber = dto.PhoneNumber;
            company.Address = dto.Address;
            company.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(company);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateStatusAsync(
            int id, string status, string? rejectionReason, int adminUserId)
        {
            var company = await _repository.GetByIdAsync(id);

            if (company == null)
                return false;

            if (status != "Approved" && status != "Rejected")
                throw new Exception("Status must be Approved or Rejected.");

            if (status == "Rejected" && string.IsNullOrWhiteSpace(rejectionReason))
                throw new Exception("Rejection reason is required.");

            company.Status = status;
            company.RejectionReason = status == "Rejected" ? rejectionReason : null;
            company.ReviewedByUserId = adminUserId;
            company.ReviewedAt = DateTime.UtcNow;
            company.ApplicationUser.IsActive = status == "Approved";

            await _repository.UpdateAsync(company);
            await _repository.SaveChangesAsync();

            return true;
        }

        private static CompanyResponseDto MapToDto(Company company)
        {
            return new CompanyResponseDto
            {
                CompanyId = company.CompanyId,
                UserId = company.UserId,
                FullName = company.ApplicationUser.FullName,
                Email = company.ApplicationUser.Email,
                CompanyName = company.CompanyName,
                Industry = company.Industry,
                Website = company.Website,
                Description = company.Description,
                PhoneNumber = company.PhoneNumber,
                Address = company.Address,
                Status = company.Status,
                RejectionReason = company.RejectionReason,
                CreatedAt = company.CreatedAt,
                ReviewedAt = company.ReviewedAt
            };
        }
    }
}