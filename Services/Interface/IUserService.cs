using CineApi.Dto;
using CineApi.Model;

namespace CineApi.Services.Interface
{
    public interface IUserService
    {

        Task<Guid> CreateUserAsyncService(RegisterDto registerDto);
        Task<User> GetUserByIdService(Guid userId);

    }
}
