using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;

        private readonly StudentDBContext _context;

        private readonly RoleManager<IdentityRole> _roleManager;

        public LoginController(IConfiguration configuration, StudentDBContext context, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _context = context;
            _roleManager = roleManager;    
           
        }

        
        [HttpPost]
        public IActionResult Login([FromBody] Administrator? adminstrator)
        {
            var currentAdministrator = _context.Administrators.FirstOrDefault(o => o.EmailAddress.ToLower() == adminstrator.EmailAddress.ToLower()
               && o.Password == adminstrator.Password);


            if (currentAdministrator != null)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = adminstrator.Role,
                };

                var token = Generate(currentAdministrator);
                return Ok(token);    

            }
            return NotFound();
        }


        private string Generate(Administrator administrator)
        {
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier,administrator.Username),
               new Claim(ClaimTypes.Email,administrator.EmailAddress),
               new Claim(ClaimTypes.Role,administrator.Role)  
              
            };

            var token = new JwtSecurityToken(_configuration["JWT:Issuer"],
                _configuration["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: Credentials);

            return new JwtSecurityTokenHandler().WriteToken(token); 

        }

    }
}
