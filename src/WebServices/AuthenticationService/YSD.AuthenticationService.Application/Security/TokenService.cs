using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using YSD.AuthenticationService.Application.Models;
using YSD.AuthenticationService.Application.Security.Abstractions;

namespace YSD.AuthenticationService.Application.Security;

public class TokenService(
    IOptions<JwtOptions> options,
    IAccessTokenStore accessTokenStore,
    IRefreshTokenStore refreshTokenStore) : ITokenService
{
    private readonly JwtOptions _options = options.Value;

    public async Task<AccessRefreshToken> GenerateAccessRefreshTokenAsync(IdentityUser user,
        CancellationToken cancellationToken = default)
    {
        var accessToken = CreateAccessToken(user);
        var refreshToken = CreateRefreshToken();

        await accessTokenStore.RemoveAccessTokenForUserAsync(user, cancellationToken);
        await refreshTokenStore.RemoveRefreshTokenForUserAsync(user, cancellationToken);

        await accessTokenStore.SaveAccessTokenAsync(accessToken, user, cancellationToken);
        await refreshTokenStore.SaveRefreshTokenAsync(refreshToken, user, cancellationToken);

        return new AccessRefreshToken(accessToken, refreshToken);
    }

    public async Task<AccessRefreshToken?> RefreshTokenAsync(string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var user = await refreshTokenStore.GetRefreshTokenUserAsync(refreshToken, cancellationToken);

        if (user is null) return null;

        return await GenerateAccessRefreshTokenAsync(user, cancellationToken);
    }

    public async Task<bool> ValidateAccessTokenAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        return await accessTokenStore.AccessTokenExistsAsync(accessToken, cancellationToken);
    }

    private string CreateAccessToken(IdentityUser user)
    {
        var handler = new JwtSecurityTokenHandler();

        var privateKey = Encoding.UTF8.GetBytes(_options.SecretKey);

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(privateKey),
            SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(_options.ExpirationHours),
            Subject = GenerateClaims(user),
            Issuer = _options.Issuer
        };

        var token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(IdentityUser user)
    {
        var claimsIdentity = new ClaimsIdentity();

        claimsIdentity.AddClaim(new Claim(ClaimTypes.Sid, user.Id));

        return claimsIdentity;
    }

    private static string CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}