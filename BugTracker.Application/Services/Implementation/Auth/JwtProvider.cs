using BugTracker.Application.Services.Abstract.Auth;
using BugTracker.Application.Settings;
using BugTracker.Data.models.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BugTracker.Application.Services.Implementation.Auth
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtSetting _jwtSetting;

        public JwtProvider(IOptions<JwtSetting> options)
        {
            _jwtSetting = options.Value;
        }
        public JwtResult GenerateTaken(User user, IEnumerable<string> roles)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName , user.FullName),
                new Claim(JwtRegisteredClaimNames.Email , user.Email),
                new Claim(nameof(roles) , JsonSerializer.Serialize(roles)),
            };
            var symmatricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key));
            var crediential = new SigningCredentials(symmatricSecurityKey, SecurityAlgorithms.HmacSha256);
            var expiredIn = _jwtSetting.ExpiryMinutes;
            var token = new JwtSecurityToken
            (
               issuer: _jwtSetting.Issuer,
               audience: _jwtSetting.Audience,
               expires: DateTime.UtcNow.AddMinutes(expiredIn),
               signingCredentials: crediential,
               claims: claims
                );
            return new JwtResult(new JwtSecurityTokenHandler().WriteToken(token), expiredIn);
        }

        public string? ValidateTaken(string taken)
        {
            var handlers = new JwtSecurityTokenHandler();
            try
            {
                handlers.ValidateToken(taken, new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key)),
                }, out SecurityToken securityToken);
                var jwtToken = (JwtSecurityToken)securityToken;
                return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
