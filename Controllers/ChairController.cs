using CineApi.Dto;
using CineApi.Model;
using CineApi.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace CineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChairController : ControllerBase
    {
        private readonly IChairService chairService;

        public ChairController(IChairService chairService)
        {
            this.chairService = chairService;
        }

        [HttpGet("{movieId}")]
        public async Task<ActionResult <List<Chair>?> > AllChairMovie(Guid movieId)
        {

            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("user not logged");
            }
            return await chairService.GetAllChairs(movieId);
        }
    }
}
