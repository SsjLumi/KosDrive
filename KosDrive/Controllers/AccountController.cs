using KosDrive.DTOs;
using KosDrive.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KosDrive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("register/driver")]
        public async Task<IActionResult> RegisterDriver([FromBody] DriverRegisterDto dto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return Conflict("A user with this email already exists");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                FrontCarLicensePlatePicture = dto.FrontCarLicensePlatePicture,
                BackCarLicensePlatePicture = dto.BackCarLicensePlatePicture,
                DriversLicensePicture = dto.DriversLicensePicture,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if(!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Driver");
            return Ok("Driver registered successfully");
        }

        [HttpPost("register/rider")]
        public async Task<IActionResult> RegisterRider([FromBody] RiderRegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return Conflict("A user with this email already exists.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.UserName,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                Birthday = dto.Birthday.Ticks,
                IsAvailable = true,
                IsActive = false,
                IsVerified = false,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Rider");
            return Ok("Rider registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LogInDto dto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Invalid email or password");

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
                return Unauthorized("Invalid email or password");

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                userId = user.Id,
                email = user.Email,
            });
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("userType", user.UserType.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
