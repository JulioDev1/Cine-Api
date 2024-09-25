using CineApi.Context;
using CineApi.Dto;
using CineApi.Model;
using CineApi.Repositories.Interface;
using Dapper;
using System.Data;


namespace CineApi.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly IDbConnection connection;

        public AdminRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            this.connection = appDbContext.CreateConnection();
        }
        public async Task<Guid> CreateMovieByAdmin(MovieDto movieDto, Guid userId, int quantity)
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
        private async Task<int> VerifyMovieByAdmin(Guid movieId, Guid userId, IDbTransaction transaction) 
        {
            var checkQuery = @"SELECT COUNT(1) 
                               FROM UserMovies um
                               JOIN Users u ON  u.id = um.userid 
                               WHERE um.movieid = @MovieId AND u.id = @UserId
            ";

            var MovieAdmin = await connection.ExecuteScalarAsync<int>(checkQuery, new { MovieId = movieId, UserId = userId }, transaction);

            if (MovieAdmin == 0)
            {
                throw new UnauthorizedAccessException("dont have permission");
            }
            return MovieAdmin;
        }

        public async Task<Movie> UpdateMovieAdmin(MovieDto movieDto, Guid Id, Guid userId)
        {
            if (connection.State == ConnectionState.Closed) // Verifica se a conexão está fechada
            {
                connection.Open(); // Abre a conexão se necessário
            }

            using (var transaction = connection.BeginTransaction())
            {
                var MovieAdmin = await VerifyMovieByAdmin(Id, userId, transaction);

                if(MovieAdmin == 0)
                {
                    throw new UnauthorizedAccessException("dont have permission");
                }

                var updateQuery = @"UPDATE movies
                                    SET title = @Title, 
                                    description = @Description, 
                                    agerange = @AgeRange, 
                                    genre = @Genre, 
                                    eventday = @EventDay
                                    WHERE id = @Id
                ";

                var movie = new Movie
                {
                    Id = Id,
                    Title = movieDto.Title,
                    Description = movieDto.Description,
                    AgeRange = movieDto.AgeRange,
                    Genre = movieDto.Genre,
                    EventDay = movieDto.EventDay,
                };

                await connection.ExecuteAsync(updateQuery, movie, transaction);

                transaction.Commit();

                var selectQuery = @"SELECT * FROM movies WHERE id = @MovieId";
                var updatedMovie = await connection.QuerySingleAsync<Movie>(selectQuery, new { MovieId = Id });

                return updatedMovie;
            };
        }

        public async Task<string> DeleteMovieAdmin(Guid Id, Guid userId)
        {
            if (connection.State == ConnectionState.Closed) // Verifica se a conexão está fechada
            {
                connection.Open(); // Abre a conexão se necessário
            }

            using (var transaction = connection.BeginTransaction())
            {
                var MovieAdmin = await VerifyMovieByAdmin(Id, userId, transaction);

                if (MovieAdmin == 0)
                {
                    throw new UnauthorizedAccessException("dont have permission");
                }
                
                var deleteRowUserAndMoviesById = @"
                    DELETE FROM usermovies WHERE movieid = @MovieId
                ";

                await connection.ExecuteAsync(deleteRowUserAndMoviesById,new { MovieId = Id } );

                var deleteMovieAndChairsQuery = @"DELETE FROM chairs WHERE userId = @userId AND movieId = @Id";

                await connection.ExecuteAsync(deleteMovieAndChairsQuery, new { UserId = userId, Id });

                var deleteMoviesQuery = @"DELETE FROM movies WHERE id = @Id";

                await connection.ExecuteAsync(deleteMoviesQuery, Id);
                
                transaction.Commit();

                return "deleted with success";
            }
        }
    }
}
