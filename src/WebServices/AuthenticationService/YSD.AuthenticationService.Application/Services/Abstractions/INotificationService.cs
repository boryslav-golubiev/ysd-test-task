using Microsoft.AspNetCore.Identity;

namespace YSD.AuthenticationService.Application.Services.Abstractions;

public interface INotificationService
{
    Task NotifyOnUserLoggedIn(IdentityUser user, CancellationToken cancellationToken = default);
}