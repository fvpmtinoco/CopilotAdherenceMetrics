using CopilotAdherence.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CopilotAdherence.Controllers
{
    public interface ILoginService
    {
        Task<JwtSecurityToken?> ValidCredentials(string username, string password);
    }

    public class LoginService : ILoginService
    {
        private readonly Jwt _jwtSettings;

        public LoginService(IOptions<Jwt> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public Task<JwtSecurityToken?> ValidCredentials(string username, string password)
        {
            // Validate the user's credentials (this is just a placeholder - replace with your actual validation logic)
            if (username == "github" && password == "copilot")
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_jwtSettings.Issuer,
                    _jwtSettings.Audience,
                    claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Task.FromResult<JwtSecurityToken?>(token);
            }

            return Task.FromResult<JwtSecurityToken?>(null);
        }
    }
}
