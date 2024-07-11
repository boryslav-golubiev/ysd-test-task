using Microsoft.AspNetCore.Identity;

namespace YSD.AuthenticationService.Application.Services.Abstractions;

public interface IAuthenticationService
{
    Task<IdentityUser?> AuthenticateUserAsync(
        string login,
        string password,
        CancellationToken cancellationToken = default);
}