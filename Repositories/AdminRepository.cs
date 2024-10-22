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
                        UserId = userId,
                    };

                    var movieId = await connection.ExecuteScalarAsync<Guid>(
                        @"INSERT INTO Movies(title, description, ageRange, genre, eventDay, userid)
                          VALUES (@Title, @Description, @AgeRange, @Genre, @EventDay, @UserId) RETURNING Id
                        ", movies, transaction);


                    for (var i = 0; i < quantity; i++)
                    {
                        var chairs = new Chair
                        {
                            MovieId = movieId,
                            Availibility = false,
                        };
                        await connection.ExecuteAsync(
                            @"INSERT INTO Chairs(movieId, availibility) VALUES(@MovieId, @Availibility)",
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
     

        public async Task<Movie> UpdateMovieAdmin(MovieDto movieDto, Guid Id, Guid userId)
        {
            if (connection.State == ConnectionState.Closed) // Verifica se a conexão está fechada
            {
                connection.Open(); // Abre a conexão se necessário
            }

            using (var transaction = connection.BeginTransaction())
            {
                var updateQuery = @"UPDATE movies
                                    SET title = @Title, 
                                    description = @Description, 
                                    agerange = @AgeRange, 
                                    genre = @Genre, 
                                    eventday = @EventDay
                                    WHERE id = @Id AND userid = @UserId
                ";

                var movie = new 
                {
                    Title = movieDto.Title,
                    Description = movieDto.Description,
                    AgeRange = movieDto.AgeRange,
                    Genre = movieDto.Genre,
                    EventDay = movieDto.EventDay,
                    Id = Id, 
                    UserId = userId,
                };

                await connection.ExecuteAsync(updateQuery, movie, transaction);

                transaction.Commit();

                var selectQuery = @"SELECT * FROM movies WHERE Id = @MovieId";
                var updatedMovie = await connection.QuerySingleAsync<Movie>(selectQuery, new { MovieId = Id, UserId =  userId });

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
                var deleteChairs = @"
                DELETE FROM chairs WHERE movieId = @Id
                ";
                await connection.ExecuteAsync(deleteChairs, new { Id }, transaction: transaction);

                // Em seguida, exclua o filme
                var deleteMovie = @"
                DELETE FROM movies WHERE id = @Id AND userid = @UserId
                ";
                await connection.ExecuteAsync(deleteMovie, new { Id, UserId = userId }, transaction: transaction);

                transaction.Commit();

                return "deleted with success";
            }
        }



        //public async Task<Movie> AllMoviesByAdmin(Guid Id, Guid userId)
        //{
        //    if (connection.State == ConnectionState.Closed) // Verifica se a conexão está fechada
        //    {
        //        connection.Open(); // Abre a conexão se necessário
        //    }

        //    using (var transaction = connection.BeginTransaction())
        //    {
        //        var MovieAdmin = await VerifyMovieByAdmin(Id, userId, transaction);

        //        if (MovieAdmin == 0)
        //        {
        //            throw new UnauthorizedAccessException("dont have permission");
        //        }

                
        //    }
        //}
    }
}
