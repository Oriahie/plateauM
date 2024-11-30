using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PlateauMed.Infrastructure.Interfaces.Services;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration) => _configuration = configuration;

        public string GenerateToken(UserModel user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("FirstName",user.FirstName),
                new("LastName",user.LastName)
            };

            var expiration = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["Authentication:JwtBearer:AccessExpiration"]));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:JwtBearer:SecretKey"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var jwtSecurityToken = new JwtSecurityToken(
               issuer: _configuration["Authentication:JwtBearer:Issuer"],
               audience: _configuration["Authentication:JwtBearer:Audience"],
               claims: claims,
               expires: expiration,
               signingCredentials: cred
               );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
