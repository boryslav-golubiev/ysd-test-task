namespace YSD.Client.Cli.Services.Abstractions;

public interface ICredsStorage
{
    AccessRefreshToken? GetStoredTokens();
    void SaveTokens(string accessToken, string refreshToken);
}