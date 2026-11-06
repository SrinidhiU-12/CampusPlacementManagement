using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Companies
{
    public class CompanyUpdateDto
    {
        [Required]
        public string CompanyName { get; set; } = string.Empty;

        public string Industry { get; set; } = string.Empty;

        public string Website { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;
    }
}