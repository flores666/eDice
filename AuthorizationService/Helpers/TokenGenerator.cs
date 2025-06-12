using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthorizationService.Configuration;
using AuthorizationService.Models.Tokens;
using Infrastructure.AuthorizationService.Models;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationService.Helpers;

public static class TokenGenerator
{
    public static string GenerateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtGeneratorOptions.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: JwtGeneratorOptions.Issuer,
            audience: JwtGeneratorOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(Constants.AccessTokenLifeTime),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static TokenModel CreateTokens(User user, string? userIp)
    {
        return new TokenModel
        {
            AccessToken = GenerateAccessToken(user),
            RefreshToken = GenerateRefreshToken(user.Id, userIp)
        };
    }

    public static RefreshToken GenerateRefreshToken(Guid userId, string? userIp) => new RefreshToken
    {
        Id = Guid.NewGuid(),
        CreatedAt = DateTime.UtcNow,
        Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
        ExpiresAt = DateTime.UtcNow.Add(Constants.RefreshTokenLifeTime),
        UserId = userId,
        CreatedByIp = userIp
    };
}