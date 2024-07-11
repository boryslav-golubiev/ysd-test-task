using Cocona;
using YSD.AuthenticationService.Integration.Abstractions;
using YSD.AuthenticationService.Integration.Factory.Abstractions;

namespace YSD.Client.Cli;

public class Commands(
    IAuthenticationServiceClientFactory authenticationServiceClientFactory)
{
    private readonly IAuthenticationServiceClientV1 _authenticationServiceClient =
        authenticationServiceClientFactory.CreateV1Client();

    [Command("get-user-info",
        Description = "Get user info of currently authenticated user")]
    public Task GetUserInfoAsync()
    {
        Console.WriteLine("UserInfo");
        return Task.CompletedTask;
    }

    [Command("auth",
        Description = "Authenticate with login and password credentials")]
    public Task AuthAsync()
    {
        Console.WriteLine("UserInfo");
        return Task.CompletedTask;
    }
}