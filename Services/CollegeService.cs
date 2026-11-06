using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services;

public class CollegeService : ICollegeService
{
    private readonly ICollegeRepository _repository;

    public CollegeService(ICollegeRepository repository)
    {
        _repository = repository;
    }

    public async Task<CollegeResponseDto> RegisterAsync(
        CollegeRegistrationDto dto)
    {
        string code = dto.CollegeCode.Trim().ToUpperInvariant();

        if (await _repository.ExistsByCodeAsync(code))
            throw new InvalidOperationException(
                "A college with this code already exists.");

        var college = new College
        {
            CollegeName = dto.CollegeName.Trim(),
            CollegeCode = code,
            Address = dto.Address.Trim(),
            City = dto.City.Trim(),
            State = dto.State.Trim(),
            ContactPersonName = dto.ContactPersonName.Trim(),
            ContactEmail = dto.ContactEmail.Trim().ToLowerInvariant(),
            ContactPhone = dto.ContactPhone.Trim(),
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(college);
        await _repository.SaveChangesAsync();

        return Map(college);
    }

    public async Task<List<CollegeResponseDto>> GetAllAsync()
    {
        var colleges = await _repository.GetAllAsync();
        return colleges.Select(Map).ToList();
    }

    public async Task<CollegeResponseDto?> GetByIdAsync(int id)
    {
        var college = await _repository.GetByIdAsync(id);
        return college == null ? null : Map(college);
    }

    public async Task<bool> UpdateAsync(int id, CollegeUpdateDto dto)
    {
        var college = await _repository.GetByIdAsync(id);

        if (college == null)
            return false;

        college.CollegeName = dto.CollegeName.Trim();
        college.Address = dto.Address.Trim();
        college.City = dto.City.Trim();
        college.State = dto.State.Trim();
        college.ContactPersonName = dto.ContactPersonName.Trim();
        college.ContactEmail = dto.ContactEmail.Trim().ToLowerInvariant();
        college.ContactPhone = dto.ContactPhone.Trim();

        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateStatusAsync(
        int id, CollegeStatusUpdateDto dto, int adminUserId)
    {
        var college = await _repository.GetByIdAsync(id);

        if (college == null)
            return false;

        string status = dto.Status.Trim();

        if (status is not ("Approved" or "Rejected"
            or "Suspended" or "Inactive"))
        {
            throw new ArgumentException("Invalid college status.");
        }

        if (status == "Rejected" &&
            string.IsNullOrWhiteSpace(dto.RejectionReason))
        {
            throw new ArgumentException(
                "A rejection reason is required.");
        }

        if (college.Status == "Rejected" && status != "Inactive")
        {
            throw new InvalidOperationException(
                "A rejected college cannot be reactivated through this endpoint.");
        }

        if (college.Status == "Pending" &&
            status is "Suspended" or "Inactive")
        {
            throw new InvalidOperationException(
                "A pending college must first be approved or rejected.");
        }

        college.Status = status;
        college.RejectionReason =
            status == "Rejected" ? dto.RejectionReason!.Trim() : null;
        college.ReviewedAt = DateTime.UtcNow;
        college.ReviewedByUserId = adminUserId;

        await _repository.SaveChangesAsync();
        return true;
    }

    private static CollegeResponseDto Map(College college)
    {
        return new CollegeResponseDto
        {
            CollegeId = college.CollegeId,
            CollegeName = college.CollegeName,
            CollegeCode = college.CollegeCode,
            Address = college.Address,
            City = college.City,
            State = college.State,
            ContactPersonName = college.ContactPersonName,
            ContactEmail = college.ContactEmail,
            ContactPhone = college.ContactPhone,
            Status = college.Status,
            CreatedAt = college.CreatedAt,
            ReviewedAt = college.ReviewedAt,
            RejectionReason = college.RejectionReason
        };
    }
}