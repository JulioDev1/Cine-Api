using CineApi.Dto;
using CineApi.Model;
using System.Security.Claims;

namespace CineApi.Services.Interface
{
    public interface IAuthService
    {
        Task<string> GenerateToken(LoginDto loginDto);
    }
}
