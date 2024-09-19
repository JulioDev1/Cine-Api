using CineApi.Context;
using CineApi.Dto;
using CineApi.Model;
using CineApi.Repositories.Interface;
using Dapper;
using System.Data;
using System.Runtime.CompilerServices;

namespace CineApi.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly IDbConnection connection;
        public AdminRepository( AppDbContext appDbContext) { 
            this.appDbContext = appDbContext;
            this.connection = appDbContext.CreateConnection();
        }
        public async Task<Guid> CreateMovieByAdmin(MovieDto movieDto, Guid userId ,int quantity)
        {
            if (connection.State == ConnectionState.Closed) // Verifica se a conexão está fechada
            {
                connection.Open(); // Abre a conexão se necessário
            }
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var movies = new Movie
                    {
                        Title = movieDto.Title,
                        AgeRange = movieDto.AgeRange,
                        Description = movieDto.Description,
                        Genre = movieDto.Genre,
                        EventDay = movieDto.EventDay,
                    };
                    var movieId = await connection.ExecuteScalarAsync<Guid>(
                        @"INSERT INTO Movies(title, description, ageRange, genre, eventDay)
                    VALUES (@Title, @Description, @AgeRange, @Genre, @EventDay) RETURNING Id
                    ", movies, transaction);


                    for (var i = 0; i < quantity; i++)
                    {
                        var chairs = new Chair
                        {
                            MovieId = movieId,
                            UserId = userId,
                            Availibility = false,
                        };
                        await connection.ExecuteAsync(
                            @"INSERT INTO Chairs(movieId, userId, availibility) VALUES(@MovieId,@UserId, @Availibility)",
                             chairs, transaction);
                    }
                    transaction.Commit();
                    return movieId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("error", ex);
                }
                
            }
        }
        public async Task<Guid> AssociateMovieByAdmin(UserMovies userMovies)
        {

            if (connection.State == ConnectionState.Closed) // Verifica se a conexão está fechada
            {
                connection.Open(); // Abre a conexão se necessário
            }
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var query = @"INSERT INTO UserMovies(userId, movieId) 
                    VALUES (@UserId, @MovieId) RETURNING Id";

                    var result = await connection.ExecuteScalarAsync<Guid>(query, userMovies, transaction);
                    transaction.Commit(); 
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("error", ex);
                };
            }
        }

    }
}
