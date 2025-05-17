using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Coin_Exchange.Configuration
{
    public class JwtProviders
    {
        private static readonly string SecretKey = "sgryuodhadfeyuoaskjcdyiepyicayulkl";
        private static readonly SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

        public static string GenerateToken(string email, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email)
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "Trading_coin",
                audience: "Trader",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static string GetEmailFromToken(string token)
        {
            
            var cleanToken = token.Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(cleanToken);
            var emailClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            return emailClaim?.Value;
        }
    }
}
