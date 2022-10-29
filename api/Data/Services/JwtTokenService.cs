using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Data.Services
{
    public interface IJwtTokenService
    {
        string CreateAccessToken(string id, string email, IEnumerable<string> userRoles);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly SymmetricSecurityKey authSigningKey;
        private readonly string issuer;
        private readonly string audience;

        public JwtTokenService(IConfiguration configuration)
        {
            authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            issuer = configuration["JWT:ValidIssuer"];
            audience = configuration["JWT:ValidAudience"];
        }

        public string CreateAccessToken(string id, string email, IEnumerable<string> userRoles)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, id)
            };

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var accessToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.UtcNow.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
           );
            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }
    }
}
