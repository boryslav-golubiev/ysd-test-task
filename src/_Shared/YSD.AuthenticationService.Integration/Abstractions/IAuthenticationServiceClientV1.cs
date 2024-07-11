using YSD.AuthenticationService.Integration.Models;

namespace YSD.AuthenticationService.Integration.Abstractions;

public interface IAuthenticationServiceClientV1
{
    bool IsAuthenticated { get; }
    Task<AuthenticationResult> AuthenticatedAsync(string login, string password);
    Task<AuthenticationResult> AuthenticatedWithAccessRefreshTokenAsync(string accessToken, string refreshToken);
    void ForgetUser();
    Task<UserInfo> GetUserInfoAsync();
}