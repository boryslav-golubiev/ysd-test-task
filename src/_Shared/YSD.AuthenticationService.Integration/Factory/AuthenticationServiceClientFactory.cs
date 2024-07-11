using YSD.AuthenticationService.Integration.Abstractions;
using YSD.AuthenticationService.Integration.Factory.Abstractions;

namespace YSD.AuthenticationService.Integration.Factory;

public class AuthenticationServiceClientFactory(
    AuthenticationServiceClientFactoryConfiguration configuration,
    IHttpClientFactory httpClientFactory) : IAuthenticationServiceClientFactory
{
    public IAuthenticationServiceClientV1 CreateV1Client()
    {
        var httpClient = httpClientFactory.CreateClient(Constants.HttpClientName);
        var options = new AuthenticationServiceClientV1Options(configuration.BaseUrl);

        return new AuthenticationServiceClientV1(httpClient, options);
    }
}