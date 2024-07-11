using YSD.AuthenticationService.Integration.Abstractions;

namespace YSD.AuthenticationService.Integration.Factory.Abstractions;

public interface IAuthenticationServiceClientFactory
{
    IAuthenticationServiceClientV1 CreateV1Client();
}