
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations
{
    public class PlacementOfficerRepository : IPlacementOfficerRepository
    {
        private readonly ApplicationDbContext _context;

        public PlacementOfficerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Check duplicate email
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.ApplicationUsers
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        // Check whether college exists and is approved
        public async Task<bool> CollegeExistsAndApprovedAsync(int collegeId)
        {
            return await _context.Colleges
                .AnyAsync(c => c.CollegeId == collegeId &&
                               c.Status == "Approved");
        }

        // Add placement officer
        public async Task AddAsync(PlacementOfficer placementOfficer)
        {
            await _context.PlacementOfficers.AddAsync(placementOfficer);
        }

        // Get officer by ID
        public async Task<PlacementOfficer?> GetByIdAsync(int id)
        {
            return await _context.PlacementOfficers
                .Include(p => p.ApplicationUser)
                .Include(p => p.College)
                .FirstOrDefaultAsync(p =>
                    p.PlacementOfficerId == id);
        }

        // Get officer by user ID
        public async Task<PlacementOfficer?> GetByUserIdAsync(int userId)
        {
            return await _context.PlacementOfficers
                .Include(p => p.ApplicationUser)
                .Include(p => p.College)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        // Get all placement officers
        public async Task<IEnumerable<PlacementOfficer>> GetAllAsync()
        {
            return await _context.PlacementOfficers
                .Include(p => p.ApplicationUser)
                .Include(p => p.College)
                .ToListAsync();
        }

        // Update placement officer
        public Task UpdateAsync(PlacementOfficer placementOfficer)
        {
            _context.PlacementOfficers.Update(placementOfficer);
            return Task.CompletedTask;
        }

        // Save database changes
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
