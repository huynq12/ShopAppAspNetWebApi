using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopApp.Models.Users;
using ShopApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace ShopApp.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager,
			IConfiguration configuration,
			IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
		}
		[HttpGet("/users")]
		public async Task<IActionResult> GetUsers()
		{
			var listUsers = await _userManager.Users.ToListAsync();
			return Ok(listUsers);
		}

		[HttpGet("/profile")]
		public async Task<IActionResult> GetUserInfo()
		{
			var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
			if (userName == null)
				return BadRequest();
			
			var user = await _userManager.FindByNameAsync(userName);
			if(user == null)
				return NotFound();
			return Ok(user);
		}

		[HttpPut("/update-user")]
		public async Task<IActionResult> Update()


		[HttpPost("/login")]

		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			var user = await _userManager.FindByNameAsync(model.Email);
			if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
			{
				var userRoles = await _userManager.GetRolesAsync(user);

				var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.Email),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				};

				foreach (var userRole in userRoles)
				{
					authClaims.Add(new Claim(ClaimTypes.Role, userRole));
				}

				var token = GetToken(authClaims);

				return Ok(new
				{
					token = new JwtSecurityTokenHandler().WriteToken(token),
					expiration = token.ValidTo.AddDays(1)
				});
			}
			return Unauthorized();
		}
		

		[HttpPost("/register")]

		public async Task<IActionResult> Register([FromBody] RegisterModel model)
		{
			var userExists = await _userManager.FindByEmailAsync(model.Email);
			if (userExists != null)
				return StatusCode(StatusCodes.Status500InternalServerError, new UserManagerResponse
				{
					Status = "Error",
					Message = "User already exists!"
				});

			ApplicationUser user = new()
			{
				Email = model.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = model.Email
			};
			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
				return StatusCode(StatusCodes.Status500InternalServerError, new UserManagerResponse
				{
					Status = "Error",
					Message = "User creation failed! Please check user details and try again."
				});

			return Ok(new UserManagerResponse
			{
				Status = "Success",
				Message = "User created successfully!"
			});
		}

		[HttpPost("/register-admin")]
		public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
		{
			var userExists = await _userManager.FindByEmailAsync(model.Email);
			if (userExists != null)
				return StatusCode(StatusCodes.Status500InternalServerError,
					new UserManagerResponse
					{
						Status = "Error",
						Message = "User already exists!"
					});

			ApplicationUser user = new()
			{
				Email = model.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = model.Email
			};
			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
				return StatusCode(StatusCodes.Status500InternalServerError,
					new UserManagerResponse
					{
						Status = "Error",
						Message = "User creation failed! Please check user details and try again."
					});

			if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
				await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
			if (!await _roleManager.RoleExistsAsync(UserRoles.User))
				await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

			if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
			{
				await _userManager.AddToRoleAsync(user, UserRoles.Admin);
			}
			if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
			{
				await _userManager.AddToRoleAsync(user, UserRoles.User);
			}
			return Ok(new UserManagerResponse
			{
				Status = "Success",
				Message = "User created successfully!"
			});
		}

		private JwtSecurityToken GetToken(List<Claim> authClaims)
		{
			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

			var token = new JwtSecurityToken(
				//issuer: _configuration["JWT:ValidIssuer"],
				//audience: _configuration["JWT:ValidAudience"],
				expires: DateTime.Now.AddHours(3),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
				);

			return token;
		}
	}
}
