using CineApi.Dto;
using CineApi.Model;

namespace CineApi.Services.Interface
{
    public interface IAdminService
    {
        Task<Guid> CreateMovieAndChairs(MovieDto movieDto, Guid userId, int qtd);
        Task<Movie> UpdateMovieAssocieted(MovieDto movieDto, Guid userId, Guid movieId);
        Task<string> DeleteMovieAdmin(Guid Id, Guid userId);
    }
}
