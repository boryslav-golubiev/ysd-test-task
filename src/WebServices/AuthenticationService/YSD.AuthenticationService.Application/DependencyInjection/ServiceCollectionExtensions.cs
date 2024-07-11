using Microsoft.Extensions.DependencyInjection;
using YSD.AuthenticationService.Application.Security;
using YSD.AuthenticationService.Application.Security.Abstractions;
using YSD.AuthenticationService.Application.Services;
using YSD.AuthenticationService.Application.Services.Abstractions;

namespace YSD.AuthenticationService.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(
        this IServiceCollection services)
    {
        #region Security

        services
            .AddOptions<JwtOptions>()
            .BindConfiguration(Constants.JwtConfigurationSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IAccessTokenStore, AccessTokenStoreInMemory>();
        services.AddSingleton<IRefreshTokenStore, RefreshTokenStoreInMemory>();
        services.AddScoped<ITokenService, TokenService>();

        #endregion Security

        #region Application

        services.AddScoped<IAuthenticationService, Services.AuthenticationService>();
        services.AddScoped<IUserInfoService, UserInfoService>();
        services.AddScoped<INotificationService, NotificationService>();

        #endregion Application
    }
}