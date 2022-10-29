using api.Authorization.Model;
using api.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Authorization
{
    public interface ITokenManager
    {
        Task<string> CreateAccessTokenAsync(RegisteredUser user);
    }

    public class TokenManager : ITokenManager
    {
        private SymmetricSecurityKey authSigningKey;
        private UserManager<RegisteredUser> userManager;
        public TokenManager(IConfiguration configuration, UserManager<RegisteredUser> userManager)
        {
            authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            userManager = userManager;
        }

        public async Task<string> CreateAccessTokenAsync(RegisteredUser user)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(CustomClaims.UserId, user.Id.ToString())
            };
            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var accessSecurityToken = new JwtSecurityToken
            (
                expires: DateTime.UtcNow.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)

            );
            return new JwtSecurityTokenHandler().WriteToken(accessSecurityToken);
        }
    }
}
