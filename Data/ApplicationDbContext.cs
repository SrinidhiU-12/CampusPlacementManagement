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
        public DbSet<PlacementOfficer> PlacementOfficers { get; set; }
        
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

            // PlacementOfficer: One-to-One with ApplicationUser
            modelBuilder.Entity<PlacementOfficer>()
                .HasOne(p => p.ApplicationUser)
                .WithOne()
                .HasForeignKey<PlacementOfficer>(p => p.UserId);

            // PlacementOfficer: Many-to-One with College
            modelBuilder.Entity<PlacementOfficer>()
                .HasOne(p => p.College)
                .WithMany()
                .HasForeignKey(p => p.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique PlacementOfficer UserId
            modelBuilder.Entity<PlacementOfficer>()
                .HasIndex(p => p.UserId)
                .IsUnique();
        }
    }
}