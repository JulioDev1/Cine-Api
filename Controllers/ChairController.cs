using CineApi.Dto;
using CineApi.Model;
using CineApi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<ActionResult <List<Chair>?>> AllChairMovie(Guid movieId)
        {

            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("user not logged");
            }
            var chairs =  await chairService.GetAllChairs(movieId);
            
            return Ok(chairs);
        }
        [HttpPatch("reserve-chair/{id}")]
        [Authorize]
        public async Task <ActionResult<Guid>> ReserveChairForUser(Guid id)
        {
            var Id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            if (Id == null)
            {
                return Unauthorized("user not logged");
            }
            var userId = Guid.Parse(Id.Value);
            
            var guid = await chairService.ReserveChairForUser(userId, id);

            return Ok(guid);
        }
        [HttpGet("chair/{id}")]
        [Authorize]
        public async Task<ActionResult<Chair>> GetChairById(Guid id)
        {
            var Id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            if (Id == null)
            {
                return Unauthorized("user not logged");
            }
            var userId = Guid.Parse(Id.Value);

            var chair = await chairService.GetChairById(id);

            return Ok(chair);
        }
    }
}
