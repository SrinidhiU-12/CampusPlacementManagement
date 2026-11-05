
using System.Security.Cryptography;

namespace CampusPlacementManagement.Services
{
    public class PasswordService
    {
        private const int Iterations = 600000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize
            );

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }
    }
}
