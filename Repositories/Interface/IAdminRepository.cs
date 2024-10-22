using CineApi.Dto;
using CineApi.Model;

namespace CineApi.Repositories.Interface
{
    public interface IAdminRepository
    {
        Task<Guid> CreateMovieByAdmin(MovieDto movieDto, Guid userId, int quantity);
        Task<Movie> UpdateMovieAdmin(MovieDto movieDto, Guid movieId, Guid userId);
        Task<string> DeleteMovieAdmin(Guid Id, Guid userId);
        Task<List<Movie>> AllMoviesByAdmin(Guid userId);
        Task <Movie> GetMovieById(Guid Id, Guid userId);

    }
}
