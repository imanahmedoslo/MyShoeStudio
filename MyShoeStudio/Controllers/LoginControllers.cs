using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MyShoeStudio.Data.Models;
using System.Security.Claims;


namespace MyShoeStudio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginControllers : ControllerBase
    {
        private IConfiguration _config;
        private UserManager<User> _userManager;

        public LoginControllers(IConfiguration config, UserManager<User> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, loginRequest.Password)))
            {
                return NotFound();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("id", user.Id),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User"),
                // Add more claims as needed
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(120),
                SigningCredentials = credentials,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Issuer"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                AccessToken = tokenHandler.WriteToken(token),
                ExpiresAt = token.ValidTo,
            });
        }
    }
}
