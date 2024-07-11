using Microsoft.AspNetCore.Identity;

namespace YSD.AuthenticationService.Application.Security.Abstractions;

public interface IAccessTokenStore
{
    Task SaveAccessTokenAsync(string accessToken, IdentityUser user,
        CancellationToken cancellationToken = default);

    Task RemoveAccessTokenAsync(string accessToken,
        CancellationToken cancellationToken = default);

    Task RemoveAccessTokenForUserAsync(IdentityUser user,
        CancellationToken cancellationToken = default);

    Task<bool> AccessTokenExistsAsync(string accessToken,
        CancellationToken cancellationToken = default);
}