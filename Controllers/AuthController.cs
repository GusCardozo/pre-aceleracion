using Challenge.Aceleracion.Context;
using Challenge.Aceleracion.Entities;
using Challenge.Aceleracion.Utilities;
using Challenge.Aceleracion.ViewModels.Auth.Login;
using Challenge.Aceleracion.ViewModels.Auth.Register;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Aceleracion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtOptions _options;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<User> userManager,
            IOptions<JwtOptions> options,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _options = options.Value;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("register-user")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
        {
            var userExist = await _userManager.FindByNameAsync(model.UserName);

            if (userExist != null)
            {
                return BadRequest();
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Status = "Error",
                        Message = $"User Creation Failed! Errors: {string.Join(',', result.Errors.Select(x => x.Description))}"
                    });
            }

            if (!await _roleManager.RoleExistsAsync("admin"))
                await _roleManager.CreateAsync(new IdentityRole("admin"));

            if (!await _roleManager.RoleExistsAsync("user"))
                await _roleManager.CreateAsync(new IdentityRole("user"));

            if (await _roleManager.RoleExistsAsync("user"))
                await _userManager.AddToRoleAsync(user, "user");

            return Ok(new
            {
                Status = "Success",
                Message = "User Created Successfully"
            });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequestModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.UserName);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new AuthenticationResponseModel 
                    { 
                        Status = "Error", 
                        Message = "User already exists!" 
                    });
            }

            User user = new()
            {
                Email = model.Email,
                UserName = model.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            { return StatusCode(StatusCodes.Status500InternalServerError,
                    new AuthenticationResponseModel
                    {
                        Status = "Error",
                        Message = "User creation failed! Please check user details and try again"
                    });
            }

            if (!await _roleManager.RoleExistsAsync("admin"))
                await _roleManager.CreateAsync(new IdentityRole("admin"));

            if (!await _roleManager.RoleExistsAsync("user"))
                await _roleManager.CreateAsync(new IdentityRole("user"));

            if (await _roleManager.RoleExistsAsync("admin"))
                await _userManager.AddToRoleAsync(user, "admin");

            return Ok(new AuthenticationResponseModel
            {
                Status = "Seccess",
                Message = "User created successfully"
            });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized();

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var token = new JwtSecurityToken(
                issuer: _options.ValidIssuer,
                audience: _options.ValidAudience,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}
