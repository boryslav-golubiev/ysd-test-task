using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using YSD.AuthenticationService.Integration.Abstractions;
using YSD.AuthenticationService.Integration.Models;

namespace YSD.AuthenticationService.Integration;

public class AuthenticationServiceClientV1(
    HttpClient httpClient,
    AuthenticationServiceClientV1Options options) : IAuthenticationServiceClientV1
{
    private const string AuthenticationUrl = "v1/authentication/auth";
    private const string TokenValidateUrl = "v1/token/validate";
    private const string RefreshTokenUrl = "v1/token/refresh";
    private const string GetUserInfoUrl = "v1/userInfo";

    private readonly Uri _baseUri = options.BaseUrl;

    private string AccessToken = string.Empty;
    private string RefreshToken = string.Empty;

    public bool IsAuthenticated =>
        string.IsNullOrEmpty(AccessToken) && string.IsNullOrEmpty(RefreshToken);

    public async Task<AuthenticationResult> AuthenticatedAsync(string login, string password)
    {
        var requestUri = new Uri(_baseUri, new Uri(AuthenticationUrl));

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        using var jsonContent = new StringContent(
            JsonSerializer.Serialize(new { login, password }),
            Encoding.UTF8,
            "application/json");

        requestMessage.Content = jsonContent;

        var request = await httpClient.SendAsync(requestMessage);

        var result = await request.Content.ReadFromJsonAsync<AuthenticationResult>();

        AccessToken = result.AccessToken;
        RefreshToken = result.RefreshToken;

        return result;
    }

    public async Task<AuthenticationResult> AuthenticatedWithAccessRefreshTokenAsync(string accessToken,
        string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;

        var isTokenValid = await ValidateAccessTokenAsync(accessToken);
        if (!isTokenValid) await RefreshTokenAsync();

        return new AuthenticationResult(AccessToken, RefreshToken);
    }

    public void ForgetUser()
    {
        AccessToken = string.Empty;
        RefreshToken = string.Empty;
    }

    public async Task<UserInfo> GetUserInfoAsync()
    {
        if (!IsAuthenticated)
            throw new InvalidOperationException(
                "Client is not authorized. " +
                "Use AuthorizeAsync(string login, string password) to authorize.");

        try
        {
            return await RequestGetUserInfoAsync();
        }
        catch (HttpRequestException exception)
        {
            if (exception.StatusCode != HttpStatusCode.Forbidden) throw;

            await RefreshTokenAsync();
            return await RequestGetUserInfoAsync();
        }
    }

    private async Task<bool> ValidateAccessTokenAsync(string accessToken)
    {
        var requestUri = new Uri(_baseUri, new Uri(TokenValidateUrl));

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var request = await httpClient.SendAsync(requestMessage);

        return request.IsSuccessStatusCode;
    }

    private async Task<UserInfo> RequestGetUserInfoAsync()
    {
        var requestUri = new Uri(_baseUri, new Uri(GetUserInfoUrl));

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

        using var request = await httpClient.SendAsync(requestMessage);

        var result = await request.Content.ReadFromJsonAsync<UserInfo>();

        return result;
    }

    private Task<AuthenticationResult> RequestRefreshTokenAsync(string refreshToken)
    {
        return Task.FromResult((AuthenticationResult)null);
    }

    private async Task RefreshTokenAsync()
    {
        var refreshResult = await RequestRefreshTokenAsync(RefreshToken);

        AccessToken = refreshResult.AccessToken;
        RefreshToken = refreshResult.RefreshToken;
    }
}