using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebsiteBuilderApi.Data;
using WebsiteBuilderApi.Models;

namespace WebsiteBuilderApi.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public AuthService(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddHours(2),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ApplicationUser> AuthenticateGoogleUserAsync(string email, string providerId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Provider == "Google");

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = email,
                    Provider = "Google",
                    OAuthId = providerId
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            return user;
        }
    }
}
