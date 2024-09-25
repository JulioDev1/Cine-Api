using CineApi.Context;
using CineApi.Dto;
using CineApi.Model;
using CineApi.Repositories.Interface;
using Dapper;
using System;
using System.Data;

namespace CineApi.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly AppDbContext appDbContext;
        private readonly IDbConnection connection;
        public UserRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            this.connection = appDbContext.CreateConnection();
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

            if (connection.State == ConnectionState.Closed) // Verifica se a conexão está fechada
            {
                connection.Open(); // Abre a conexão se necessário
            }
            using (var transaction = connection.BeginTransaction())
            {
                var query = "INSERT INTO Users (Name, Email, Password, Role) VALUES (@Name, @Email, @Password, @Role)";
               
                var result = await connection.ExecuteScalarAsync<Guid>(query, parameters);
                
                transaction.Commit();

                return result;
            }

        }

        public async Task<string?> FindUserByEmail(string Email)
        {
            if (connection.State == ConnectionState.Closed) // Verifica se a conexão está fechada
            {
                connection.Open(); // Abre a conexão se necessário
            }
            using (var transaction = connection.BeginTransaction())
            {
                var query = "SELECT Email FROM Users WHERE Email = @Email";
                var result = await connection.QueryFirstOrDefaultAsync<string>(query, new { Email });
                transaction.Commit();
                return result;
            }

        }

        public async Task<User?> GetUserByEmailAsync(string Email)
        {
            if (connection.State == ConnectionState.Closed) // Verifica se a conexão está fechada
            {
                connection.Open(); // Abre a conexão se necessário
            }
            using (var transaction = connection.BeginTransaction())
            {
                var query = "SELECT * FROM Users WHERE Email = @Email";
                var result =  await connection.QuerySingleOrDefaultAsync<User>(query, new { Email = Email });
                transaction.Commit();
                return result;
            }
        }

        public async Task<User> GetUserById(Guid id)
        {
            if (connection.State == ConnectionState.Closed) // Verifica se a conexão está fechada
            {
                connection.Open(); // Abre a conexão se necessário
            }
            using (var transaction = connection.BeginTransaction())
            {

                var query = "SELECT * FROM Users WHERE Id = @Id";
                
                var result = await connection.QuerySingleAsync<User>(query, new { Id = id });
                
                transaction.Commit();
                
                return result;
            }
        }
    }
}
