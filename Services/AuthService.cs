using CineApi.Dto;
using CineApi.Helpers;
using CineApi.Model;
using CineApi.Repositories.Interface;
using CineApi.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CineApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly ICrypthografyService crypthografyService;

        public  AuthService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            crypthografyService = new CrypthografyService();
        }

        public async Task<string> GenerateToken(LoginDto loginDto)
        {

            var user = await userRepository.GetUserByEmailAsync(loginDto.Email);

            if (user is null)
            {
                throw new Exception("User not found.");
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                throw new Exception("password input cant be empty");
            }

            var comparePassword = crypthografyService.ComparePassword(loginDto.Password, user.Password);

            if (!comparePassword)
            {

                throw new Exception("incorrectly password");
            }

            var handler = new JwtSecurityTokenHandler();
            
            var key = Encoding.ASCII.GetBytes(AuthoSetting.PrivateKey);
            
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
             );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(1),
                Subject = GenerateClaims(user)
            };
            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(User user)
        {
            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            claims.AddClaim(new Claim(ClaimTypes.Name, user.Email));

            return claims;
        }

    }
}
