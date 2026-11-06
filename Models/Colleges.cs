using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.Models;

public class College
{
    public int CollegeId { get; set; }

    [Required, MaxLength(180)]
    public string CollegeName { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string CollegeCode { get; set; } = string.Empty;

    [Required, MaxLength(250)]
    public string Address { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string City { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string State { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    public string ContactPersonName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(180)]
    public string ContactEmail { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string ContactPhone { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReviewedAt { get; set; }

    public int? ReviewedByUserId { get; set; }

    [MaxLength(500)]
    public string? RejectionReason { get; set; }
}