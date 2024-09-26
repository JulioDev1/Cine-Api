using CineApi.Dto;
using CineApi.Model;
using CineApi.Repositories.Interface;
using CineApi.Services.Interface;

namespace CineApi.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
        }
        public async Task<Guid> CreateMovieAndChairs(MovieDto movieDto, Guid userId, int qtd)
        {
           var guid = await adminRepository.CreateMovieByAdmin(movieDto, userId, qtd);

            return guid;
        }

        public async Task<string> DeleteMovieAdmin(Guid Id, Guid userId)
        {
            var deleteMovie = await adminRepository.DeleteMovieAdmin(Id, userId);

            return deleteMovie;
        }

        public async Task<Movie> UpdateMovieAssocieted(MovieDto movieDto, Guid movieId, Guid userId)
        {
            var movie = await adminRepository.UpdateMovieAdmin(movieDto, movieId, userId);
            return movie;
        }
    }
}
