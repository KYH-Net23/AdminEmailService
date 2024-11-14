using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmailProvider.EmailServices
{
    public class TokenGeneratorService
    {
        public string GenerateAccessToken(string secretKey)
        {
            var key = Encoding.UTF8.GetBytes(secretKey);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Name, "Gustav")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = "Kimmo",
                Audience = "Gurra",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(tokenDescriptor));
        }
    }
}
