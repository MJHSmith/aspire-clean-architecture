using System.Security.Claims;
using System.Text;
using Application.Abstractions.Authentication;
using Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

internal sealed class TokenProvider(IOptions<JwtOptions> jwtOptions, TimeProvider timeProvider) : ITokenProvider
{
    public string Create(User user)
    {
        string secretKey = jwtOptions.Value.Secret;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            ]),
            Expires = timeProvider.GetUtcNow().AddMinutes(
                jwtOptions.Value.ExpirationInMinutes).UtcDateTime,
            SigningCredentials = credentials,
            Issuer = jwtOptions.Value.Issuer,
            Audience = jwtOptions.Value.Audience
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}
