using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using YSD.AuthenticationService.Integration.Factory;
using YSD.AuthenticationService.Integration.Factory.Abstractions;

namespace YSD.AuthenticationService.Integration.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddAuthenticationServiceClientV1(
        this IServiceCollection services)
    {
        services
            .AddOptions<AuthenticationServiceClientFactoryConfiguration>()
            .BindConfiguration(Constants.ConfigurationSectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddHttpClient(Constants.HttpClientName);

        services
            .AddSingleton<IAuthenticationServiceClientFactory, AuthenticationServiceClientFactory>(
                serviceProvider =>
                {
                    var factoryOptions = serviceProvider
                        .GetRequiredService<IOptions<AuthenticationServiceClientFactoryConfiguration>>();
                    var httpClientFactory = serviceProvider
                        .GetRequiredService<IHttpClientFactory>();

                    return new AuthenticationServiceClientFactory(
                        factoryOptions.Value, httpClientFactory);
                });
    }
}