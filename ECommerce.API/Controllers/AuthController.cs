using ECommerce.Application.Auth;
using ECommerce.Application.Common.Security;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        private readonly JwtTokenService _tokenService;

        public AuthController(
            ECommerceDbContext context,
            JwtTokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = PasswordHasher.Hash(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login(LoginRequest request)
        //{
        //    var hash = PasswordHasher.Hash(request.Password);

        //    var user = await _context.Users
        //        .FirstOrDefaultAsync(x =>
        //            x.Email == request.Email &&
        //            x.PasswordHash == hash);

        //    if (user == null)
        //        return Unauthorized();

        //    var token = _tokenService.GenerateToken(user.Email);

        //    return Ok(new { token });
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var hash = PasswordHasher.Hash(request.Password);

                var user = await _context.Users
                    .FirstOrDefaultAsync(x =>
                        x.Email == request.Email &&
                        x.PasswordHash == hash);

                if (user == null)
                    return Unauthorized("User not found");

                var token = _tokenService.GenerateToken(user.Email);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
