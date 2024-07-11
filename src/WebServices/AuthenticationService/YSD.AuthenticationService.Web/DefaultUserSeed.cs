using Microsoft.AspNetCore.Identity;

namespace YSD.AuthenticationService.Web;

public class DefaultUserSeed(
    UserManager<IdentityUser> userManager) : IHostedService
{
    private const string DefaultUserName = "defaultUser";
    private const string DefaultPassword = "defaultPassword1$";

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var existingDefaultUser = await userManager.FindByNameAsync(DefaultUserName);

        if (existingDefaultUser is not null) return;

        var defaultUser = new IdentityUser
        {
            UserName = DefaultUserName
        };

        await userManager.CreateAsync(defaultUser, DefaultPassword);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}