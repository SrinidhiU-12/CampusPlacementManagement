
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusPlacementManagement.Models
{
    public class PlacementOfficer
    {
        [Key]
        public int PlacementOfficerId { get; set; }

        // Authentication relationship
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; } = null!;

        // College relationship
        [Required]
        public int CollegeId { get; set; }

        public College College { get; set; } = null!;

        // Officer details
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Designation { get; set; } = string.Empty;

        // Registration and approval
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public string? RejectionReason { get; set; }

        public int? ReviewedByUserId { get; set; }

        public DateTime? ReviewedAt { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
