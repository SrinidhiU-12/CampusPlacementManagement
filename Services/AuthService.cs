
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using CampusPlacementManagement.DTOs.Auth;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace CampusPlacementManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            IAuthRepository authRepository,
            IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _authRepository.GetUserByEmailAsync(request.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException(
                    "Your account is inactive.");
            }

            // Verify password using built-in PBKDF2
            bool isPasswordValid = VerifyPassword(
                request.Password,
                user.PasswordHash
            );

            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            // JWT Configuration
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var expiresAt = DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(jwtSettings["DurationInMinutes"])
            );

            // JWT Claims
            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()),

                new Claim(ClaimTypes.Name, user.FullName),

                new Claim(ClaimTypes.Email, user.Email),

                new Claim(ClaimTypes.Role, user.Role)
            };

            // Generate JWT Token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return new LoginResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expiresAt
            };
        }

        // PBKDF2 Password Verification
        private bool VerifyPassword(string password, string storedHash)
        {
            try
            {
                string[] parts = storedHash.Split('.');

                if (parts.Length != 3 ||
                    !int.TryParse(parts[0], out int iterations) ||
                    iterations < 1)
                {
                    return false;
                }

                byte[] salt = Convert.FromBase64String(parts[1]);

                byte[] expectedHash = Convert.FromBase64String(parts[2]);

                if (expectedHash.Length != 32)
                {
                    return false;
                }

                byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                    password,
                    salt,
                    iterations,
                    HashAlgorithmName.SHA256,
                    expectedHash.Length
                );

                return CryptographicOperations.FixedTimeEquals(
                    actualHash,
                    expectedHash
                );
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
