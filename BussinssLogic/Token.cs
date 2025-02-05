using BlogApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogApi.BussinssLogic
{
    
        public class TokenService
        {
            private readonly IConfiguration _configuration;
            private  readonly string _jwtKey;
            private readonly string _issuer;
            private readonly string _audience;

        public TokenService(IConfiguration configuration)
            {
                _configuration = configuration;
                _jwtKey = configuration["Jwt:Key"];
                _issuer = configuration["Jwt:Issuer"];
                 _audience = configuration["Jwt:Audience"];
            }

            public   string GenerateToken(Users user) 
            {
           
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username), 
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
            new Claim("email", user.Email) 
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        }
    }
