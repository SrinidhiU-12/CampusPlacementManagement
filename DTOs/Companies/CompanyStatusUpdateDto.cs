using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Companies
{
    public class CompanyStatusUpdateDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;

        public string? RejectionReason { get; set; }
    }
}