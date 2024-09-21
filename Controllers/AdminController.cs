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
        public async Task<ActionResult<Guid>> CreateMovieAsync([FromBody] CreateRequestMovie request)
        {
            var Id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            if (Id == null)
            {
                return Unauthorized("user not logged");
            }
            var userId = Guid.Parse(Id.Value);

            var user = await userService.GetUserByIdService(userId);

            Console.WriteLine(user.Id);

            if (user.Role != UserRole.Admin)
            {
                return Unauthorized("user not admin");
            }
            var movieId = await adminServices.CreateMovieAndChairs(request.MovieDto, user.Id, request.Qtd);

            return movieId;
        }
        [HttpPut("update-movie-admin")]
        [Authorize]
        public async Task<ActionResult<Movie>> UpdateMovieAssocieted(MovieDto movieDto ,Guid movieId)
        {
            var Id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            if (Id == null)
            {
                return Unauthorized("user not logged");
            }


            var userId = Guid.Parse(Id.Value);

            var movie = await adminServices.UpdateMovieAssocieted(movieDto, movieId, userId);

            return movie;
        }
        [HttpDelete("Delete-movie-admin")]
        [Authorize]
        public async Task<ActionResult<string>> DeleteMovieByAdmin(Guid Id)
        {
            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("user not logged");
            }

            var guid = Guid.Parse(userId.Value);

            var response = await adminServices.DeleteMovieAdmin(Id, guid);

            return response;
        }
    }
}
