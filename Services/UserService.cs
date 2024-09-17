using CineApi.Dto;
using CineApi.Model;
using CineApi.Repositories.Interface;
using CineApi.Services.Interface;

namespace CineApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private ICrypthografyService crypthografyService;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.crypthografyService = new CrypthografyService();

        }
        public async Task<Guid> CreateUserAsyncService(RegisterDto registerDto)
        {

            var email = await userRepository.FindUserByEmail(registerDto.Email);

            if (email != null)
            {
                throw new Exception("email already exist");
            }

            var passwordHash = crypthografyService.HashPassword(registerDto.Password);

            var user = new RegisterDto
            {
                Email = registerDto.Email,
                Name = registerDto.Name,
                Password = passwordHash,
                Role = registerDto.Role,
            };


            var guid = await userRepository.CreateUserAccountAsync(user);

            return guid;
        }

        public async Task<User> GetUserByIdService(Guid userId)
        {
            return await userRepository.GetUserById(userId);
        }
    }
}
