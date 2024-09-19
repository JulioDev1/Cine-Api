using CineApi.Dto;
using CineApi.Model;

namespace CineApi.Repositories.Interface
{
    public interface IAdminRepository
    {
        Task<Guid> CreateMovieByAdmin(MovieDto movieDto, Guid userId,int quantity);
        Task<Guid> AssociateMovieByAdmin(UserMovies userMovies);
        Task<Movie> UpdateMovieAdmin(MovieDto movieDto, Guid UserId, Guid MovieId);

    }
}
