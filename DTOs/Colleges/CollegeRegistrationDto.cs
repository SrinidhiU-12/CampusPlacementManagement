using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Colleges;

public class CollegeRegistrationDto
{
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

    [Required, EmailAddress]
    public string ContactEmail { get; set; } = string.Empty;

    [Required, Phone]
    public string ContactPhone { get; set; } = string.Empty;
}