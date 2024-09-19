using CineApi.Dto;
using CineApi.Model;

namespace CineApi.Repositories.Interface
{
    public interface IAdminRepository
    {
        Task<Guid> CreateMovieByAdmin(MovieDto movieDto, int quantity);
        Task<Guid> AssociateMovieByAdmin(UserMovies userMovies);

    }
}
