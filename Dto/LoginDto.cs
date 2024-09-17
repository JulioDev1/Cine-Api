using CineApi.Model;

namespace CineApi.Dto
{
    public class LoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
