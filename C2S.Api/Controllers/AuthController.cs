using C2S.Api.Models;
using C2S.Business.Interfaces;
using C2S.Business.Models;
using C2S.Data.Enumrations;
using C2S.Data.Models;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Net;

namespace C2S.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IConfiguration configuration, ITokenService tokenService)
        {
            _authService = authService;
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequest request)
        {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var userExists = await _authService.UserExistsAsync(request.Email);
            if (userExists)
                return Conflict(new { message = "User already exists." });

            bool result = await _authService.RegisterAsync(request, request.Role);

            if (!result)
            {
                return BadRequest($"Failed to register the user with Id: {request.Id}");
            }
            var token = _tokenService.CreateToken(new Data.Models.User { Email = request.Email, Name = request.Name });
            return Ok(new {message = "User registered successfully.", JwtToken = token , errorCode = HttpStatusCode.Created});
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel request)
        {
            bool result = await _authService.LoginAsync(new LoginModel
            {
                Email = request.Email,
                Password = request.Password,
            });

            if (result)
            {
                // Generate access token
                var token = _tokenService.CreateToken(new User { Email = request.Email, Name = request.Name });
                return Ok(new { message = "User logged in successfully.", JwtToken = token, errorCode = HttpStatusCode.OK } );
            }
            return Unauthorized(new { message = "Invalid email or password." });
        }
    }
}
