using IssueManager.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IssueManager.Application.Services
{
    public class TokenGenerator(IConfiguration configuration) : ITokenGenerator
    {
        public string GenerateJwtToken(string provider, string accessToken, string appUserId)
        {
            var claims = new[]
            {
                new Claim("provider", provider),
                new Claim("external_token", accessToken),
                new Claim(ClaimTypes.NameIdentifier, appUserId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
