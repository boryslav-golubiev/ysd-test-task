using System.Text.Json;
using Cocona;
using YSD.AuthenticationService.Integration.Abstractions;
using YSD.AuthenticationService.Integration.Factory.Abstractions;
using YSD.Client.Cli.Services.Abstractions;

namespace YSD.Client.Cli;

public class Commands(
    ICredsStorage credsStorage,
    IAuthenticationServiceClientFactory authenticationServiceClientFactory)
{
    private readonly IAuthenticationServiceClientV1 _authenticationServiceClient =
        authenticationServiceClientFactory.CreateV1Client();

    [Command("get-user-info",
        Description = "Get user info of currently authenticated user")]
    public async Task GetUserInfoAsync()
    {
        var tokens = credsStorage.GetStoredTokens();
        if (tokens is null)
        {
            Console.WriteLine("You need to do auth first. Do auth with login password.");
            return;
        }

        try
        {
            var authResult = await _authenticationServiceClient.AuthenticateWithAccessRefreshTokenAsync(
                tokens.AccessToken, tokens.RefreshToken);

            credsStorage.SaveTokens(tokens.AccessToken, tokens.RefreshToken);
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine("You need to do reauthorize. Do auth with login password.");
        }

        var userInfo = await _authenticationServiceClient.GetUserInfoAsync();

        Console.WriteLine($"Info!\nUserInfo:\n{JsonSerializer.Serialize(userInfo)}");
    }

    [Command("auth",
        Description = "Authenticate with login and password credentials")]
    public async Task AuthAsync([Argument] string login, [Argument] string password)
    {
        try
        {
            var tokens = await _authenticationServiceClient
                .AuthenticateAsync(login, password);

            credsStorage.SaveTokens(tokens.AccessToken, tokens.RefreshToken);
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine("Invalid login or password.");
            return;
        }

        var userInfo = await _authenticationServiceClient.GetUserInfoAsync();

        Console.WriteLine($"Authenticated!\nUserInfo:\n{JsonSerializer.Serialize(userInfo)}");
    }
}