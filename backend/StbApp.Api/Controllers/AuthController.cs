using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StbApp.Api.Data;
using StbApp.Api.Dtos.Auth;
using StbApp.Api.Services;

namespace StbApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly StbContext _db;
        private readonly JwtService _jwt;
        public AuthController(StbContext db, JwtService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username et mot de passe requis.");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);
            if (user == null) return Unauthorized("Identifiants invalides.");

            var (token, expiresAt) = _jwt.GenerateToken(user.Id, user.Username, user.Role);
            return Ok(new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                UserId = user.Id,
                ExpiresAt = expiresAt
            });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<object>> Me()
        {
            var name = User.Identity?.Name;
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == name);
            if (user == null) return NotFound();
            return Ok(new { user.Id, user.Username, user.Userfirstname, user.Email, user.Mobile, user.Role });
        }
    }
}

