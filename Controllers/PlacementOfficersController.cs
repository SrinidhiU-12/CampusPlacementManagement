
using CampusPlacementManagement.DTOs.PlacementOfficers;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacementOfficersController : ControllerBase
    {
        private readonly IPlacementOfficerService _service;

        public PlacementOfficersController(IPlacementOfficerService service)
        {
            _service = service;
        }

        // 1. Register Placement Officer
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(
            PlacementOfficerRegisterDto dto)
        {
            try
            {
                var result = await _service.RegisterAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 2. Get all Placement Officers
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // 3. Get Placement Officer by ID
        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "Placement Officer not found." });

            return Ok(result);
        }

        // 4. Get logged-in Placement Officer profile
        [HttpGet("my-profile")]
        [Authorize(Roles = "PlacementOfficer")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue("userId");

            if (!int.TryParse(userId, out int id))
                return Unauthorized();

            var result = await _service.GetByUserIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // 5. Update Placement Officer details
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update(
            int id,
            PlacementOfficerUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (!result)
                return NotFound(new { message = "Placement Officer not found." });

            return Ok(new { message = "Placement Officer updated successfully." });
        }

        // 6. Approve or Reject Placement Officer
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            PlacementOfficerStatusUpdateDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                             ?? User.FindFirstValue("userId");

                if (!int.TryParse(userId, out int adminUserId))
                    return Unauthorized();

                var result = await _service.UpdateStatusAsync(
                    id, dto, adminUserId);

                if (!result)
                    return NotFound(new { message = "Placement Officer not found." });

                return Ok(new
                {
                    message = $"Placement Officer {dto.Status} successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
