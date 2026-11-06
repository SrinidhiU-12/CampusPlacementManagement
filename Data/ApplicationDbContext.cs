using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<College> Colleges => Set<College>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ApplicationUser: Unique Email
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // College: Unique CollegeCode
            modelBuilder.Entity<College>()
                .HasIndex(c => c.CollegeCode)
                .IsUnique();
        }
    }
}