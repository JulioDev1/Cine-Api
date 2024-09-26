using CineApi.Dto;
using CineApi.Model;

namespace CineApi.Repositories.Interface
{
    public interface IAdminRepository
    {
        Task<Guid> CreateMovieByAdmin(MovieDto movieDto, Guid userId,int quantity);
        Task<Movie> UpdateMovieAdmin(MovieDto movieDto, Guid movieId);
        Task<string> DeleteMovieAdmin(Guid Id);

        //Task<Movie> AllMoviesByAdmin(Guid Id, Guid userId);

    }
}
