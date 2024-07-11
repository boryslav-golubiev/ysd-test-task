using Microsoft.AspNetCore.Identity;
using YSD.AuthenticationService.Application.Models;

namespace YSD.AuthenticationService.Application.Security.Abstractions;

public interface ITokenService
{
    Task<AccessRefreshToken> GenerateAccessRefreshTokenAsync(IdentityUser user,
        CancellationToken cancellationToken = default);

    Task<AccessRefreshToken?> RefreshTokenAsync(string refreshToken,
        CancellationToken cancellationToken = default);

    Task<bool> ValidateAccessTokenAsync(string accessToken,
        CancellationToken cancellationToken = default);
}