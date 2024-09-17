using CineApi.Dto;
using CineApi.Model;
using CineApi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpPost("register-user")]
        public async Task<ActionResult<Guid>> CreateUserAsync(RegisterDto registerDto)
        {
            if (registerDto.Role != UserRole.Admin && registerDto.Role != UserRole.User)
            {
                new Exception("user type not exists");
            }

            var guid = await userService.CreateUserAsyncService(registerDto);
            return Ok(guid);

        }
    }
}
