using CineApi.Services.Interface;

namespace CineApi.Services
{
    public class CrypthografyService : ICrypthografyService
    {

        public bool ComparePassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);

        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 10);

        }
    }
}
