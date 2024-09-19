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
           var guid = await adminRepository.CreateMovieByAdmin(movieDto, qtd);

            var MovieByAdmin = new UserMovies
            {
                MovieId = guid,
                UserId = userId,
            };

            await adminRepository.AssociateMovieByAdmin(MovieByAdmin);

            return guid;
        }
    }
}
