using Microsoft.AspNetCore.Identity;
using YSD.AuthenticationService.Application.Services.Abstractions;

namespace YSD.AuthenticationService.Application.Services;

public class AuthenticationService(
    IPasswordHasher<IdentityUser> passwordHasher,
    IUserPasswordStore<IdentityUser> userPasswordStore) : IAuthenticationService
{
    public async Task<IdentityUser?> AuthenticateUserAsync(string login, string password,
        CancellationToken cancellationToken = default)
    {
        var user = await userPasswordStore.FindByNameAsync(login, cancellationToken);

        if (user is null) return null;

        var hashedProvidedPassword = passwordHasher.HashPassword(user, password);
        var hashedActualPassword = await userPasswordStore.GetPasswordHashAsync(user, cancellationToken);

        return hashedActualPassword != hashedProvidedPassword ? null : user;
    }
}