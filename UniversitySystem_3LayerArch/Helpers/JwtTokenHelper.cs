using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using University.Core.dtos;

namespace UniversitySystem_3LayerArch.Helpers
{
    public class JwtTokenHelper : IjwtTokenHelper
    {
        private IConfiguration _config;
        public JwtTokenHelper(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateToken(UserDTO user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var keyString = jwtSettings["SecretKey"];
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString)); // ✅ FIXED HERE
            Console.WriteLine($"Secret Key: {keyString}"); // For debugging purposes, remove in production
            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email ?? " "),
        new Claim(ClaimTypes.Role, "Student"),
    };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
    public interface IjwtTokenHelper
    {
        string GenerateToken(UserDTO user);
    }
}
