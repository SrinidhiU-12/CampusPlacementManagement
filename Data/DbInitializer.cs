
using CampusPlacementManagement.Constants;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Services;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            var passwordService = scope.ServiceProvider
                .GetRequiredService<PasswordService>();

            string fullName = configuration["SeedAdmin:FullName"]
                ?? throw new InvalidOperationException("Admin name is missing.");

            string email = configuration["SeedAdmin:Email"]
                ?? throw new InvalidOperationException("Admin email is missing.");

            string password = configuration["SeedAdmin:Password"]
                ?? throw new InvalidOperationException("Admin password is missing.");

            bool adminExists = await context.ApplicationUsers
                .AnyAsync(u => u.Email == email);

            if (adminExists)
                return;

            var admin = new ApplicationUser
            {
                FullName = fullName,
                Email = email,
                PasswordHash = passwordService.HashPassword(password),
                Role = Roles.SuperAdmin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            context.ApplicationUsers.Add(admin);

            await context.SaveChangesAsync();
        }
    }
}
