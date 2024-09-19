using CineApi.Dto;
using CineApi.Model;
using CineApi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminServices;
        private readonly IUserService userService;

        public AdminController(IAdminService adminServices, IUserService userService)
        {
            this.adminServices = adminServices;
            this.userService = userService;
        }


        [HttpPost("create-movie")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]  // Sucesso
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Guid>> CreateMovieAsync(MovieDto movieDto, int qtd)
        {
            var Id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            if (Id == null)
            {
                return Unauthorized("user not logged");
            }
            var userId = Guid.Parse(Id.Value);

            var user = await userService.GetUserByIdService(userId);

            if (user.Role != UserRole.Admin)
            {
                return Unauthorized("user not admin");
            }
            var movieId = await adminServices.CreateMovieAndChairs(movieDto, user.Id, qtd);

            return movieId;
        }
    }
}
