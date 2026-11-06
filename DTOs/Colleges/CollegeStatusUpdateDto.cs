using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Colleges;

public class CollegeStatusUpdateDto
{
    [Required]
    public string Status { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? RejectionReason { get; set; }
}