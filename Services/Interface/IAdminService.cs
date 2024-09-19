using CineApi.Dto;

namespace CineApi.Services.Interface
{
    public interface IAdminService
    {
        Task<Guid> CreateMovieAndChairs(MovieDto movieDto, Guid userId, int qtd);
    }
}
