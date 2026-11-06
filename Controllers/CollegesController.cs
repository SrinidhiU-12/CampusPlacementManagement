using System.Security.Claims;
using CampusPlacementManagement.Constants;
using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollegesController : ControllerBase
{
    private readonly ICollegeService _service;

    public CollegesController(ICollegeService service)
    {
        _service = service;
    }

    // Public college registration
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        CollegeRegistrationDto dto)
    {
        try
        {
            var result = await _service.RegisterAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.CollegeId },
                result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // View all colleges
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    // View college by ID
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        return result == null ? NotFound() : Ok(result);
    }

    // Update college details
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id, CollegeUpdateDto dto)
    {
        bool updated = await _service.UpdateAsync(id, dto);

        return updated
            ? Ok(new { message = "College updated successfully." })
            : NotFound();
    }

    // Approve, reject, suspend or reactivate
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id, CollegeStatusUpdateDto dto)
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int adminUserId))
            return Unauthorized();

        try
        {
            bool updated = await _service.UpdateStatusAsync(
                id, dto, adminUserId);

            return updated
                ? Ok(new { message = "College status updated successfully." })
                : NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}