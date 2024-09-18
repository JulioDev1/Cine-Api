using CineApi.Dto;
using CineApi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CineApi.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost("user-auth")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            var AcessToken = await authService.GenerateToken(loginDto);

            if (AcessToken == "")
                return Unauthorized();
            return Ok(new
            {
                Token = AcessToken,
                Message = "login sucessfully"
            });
        }
    }
}
