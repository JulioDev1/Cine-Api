using CineApi.Dto;
using CineApi.Model;

namespace CineApi.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<Guid> CreateUserAccountAsync(RegisterDto registerDto);
        Task<string?> FindUserByEmail(string Email);
        Task<User?> GetUserByEmailAsync(string Email);
        Task<User> GetUserById(Guid id);
    }
}
