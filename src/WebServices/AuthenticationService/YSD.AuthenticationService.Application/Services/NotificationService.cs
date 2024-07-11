using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using YSD.AuthenticationService.Application.Services.Abstractions;

namespace YSD.AuthenticationService.Application.Services;

public class NotificationService(ILogger<NotificationService> logger) : INotificationService
{
    public Task NotifyOnUserLoggedIn(IdentityUser user, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("User with id {id} and login {normalizedUserName} logged in.",
            user.Id, user.NormalizedUserName);
        return Task.CompletedTask;
    }
}