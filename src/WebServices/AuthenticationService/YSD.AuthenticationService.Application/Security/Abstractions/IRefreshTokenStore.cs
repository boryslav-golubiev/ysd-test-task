using Microsoft.AspNetCore.Identity;

namespace YSD.AuthenticationService.Application.Security.Abstractions;

public interface IRefreshTokenStore
{
    Task SaveRefreshTokenAsync(string refreshToken, IdentityUser user,
        CancellationToken cancellationToken = default);

    Task RemoveRefreshTokenAsync(string refreshToken,
        CancellationToken cancellationToken = default);

    Task RemoveRefreshTokenForUserAsync(IdentityUser user,
        CancellationToken cancellationToken = default);

    Task<IdentityUser?> GetRefreshTokenUserAsync(string refreshToken,
        CancellationToken cancellationToken = default);
}