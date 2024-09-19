using CineApi.Context;
using CineApi.Dto;
using CineApi.Model;
using CineApi.Repositories.Interface;
using Dapper;

namespace CineApi.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext appDbContext;
        public AdminRepository( AppDbContext appDbContext) { 
            this.appDbContext = appDbContext;
        }
        public async Task<Guid> CreateMovieByAdmin(MovieDto movieDto, int quantity)
        {
            using (var connection = appDbContext.CreateConnection())
            {
                var movieId = await connection.QuerySingleAsync<Guid>(
                    @"INSERT INTO Movies(title, description, ageRange, genre, eventDay)
                    VALUES (@Title, @Description, @AgeRange, @Genre, EventDay) RETURNING Id
                    ");

                for(var i = 0; i < quantity; i++)
                {
                    await connection.ExecuteAsync("INSERT INTO Charis(MoviesId) VALUES(MoviesId)");
                }
                
                return movieId;
            }
        }
        public async Task<Guid> AssociateMovieByAdmin(UserMovies userMovies)
        {
            using(var connection = appDbContext.CreateConnection())
            {
                var query = @"INSERT INT UserMovies(UserId, MoviesId) 
                    VALUES (@UserId, @MoviesId) RETURNING Id";

                return await connection.QuerySingleAsync<Guid>(query, new  {Id = userMovies.Id});
            }
        }

    }
}
