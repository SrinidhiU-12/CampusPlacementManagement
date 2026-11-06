using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories;

public class CollegeRepository : ICollegeRepository
{
    private readonly ApplicationDbContext _context;

    public CollegeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _context.Colleges
            .AnyAsync(c => c.CollegeCode == code);
    }

    public async Task<List<College>> GetAllAsync()
    {
        return await _context.Colleges
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<College?> GetByIdAsync(int id)
    {
        return await _context.Colleges
            .FirstOrDefaultAsync(c => c.CollegeId == id);
    }

    public async Task AddAsync(College college)
    {
        await _context.Colleges.AddAsync(college);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}