using CineApi.Context;
using CineApi.Dto;
using CineApi.Model;
using CineApi.Repositories.Interface;
using Dapper;

namespace CineApi.Repositories
{
    public class UserRepository : IUserRepository
    {

        public readonly AppDbContext context;

        public UserRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Guid> CreateUserAccountAsync(RegisterDto registerDto)
        {

            var parameters = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = registerDto.Password,
                Role = registerDto.Role,
            };
            using (var connection = context.CreateConnection())
            {
                var query = "INSERT INTO Users (Name, Email, Password, Role) VALUES (@Name, @Email, @Password, @Role)";
                return await connection.ExecuteScalarAsync<Guid>(query, parameters);
            }

        }

        public async Task<string?> FindUserByEmail(string Email)
        {
            using (var connection = context.CreateConnection())
            {
                var query = "SELECT Email FROM Users WHERE Email = @Email";
                return await connection.QueryFirstOrDefaultAsync<string>(query, new { Email });
            }

        }

        public async Task<User?> GetUserByEmailAsync(string Email)
        {
            using (var connection = context.CreateConnection())
            {
                var query = "SELECT * FROM Users WHERE Email = @Email";
                return await connection.QuerySingleOrDefaultAsync<User>(query, new { Email = Email });
            }
        }

        public async Task<User> GetUserById(Guid id)
        {
            using (var connection = context.CreateConnection())
            {
                var query = "SELECT * FROM Users WHERE Id = @Id";
                return await connection.QuerySingleAsync<User>(query);
            }
        }
    }
}
