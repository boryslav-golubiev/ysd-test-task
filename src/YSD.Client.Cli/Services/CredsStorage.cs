using System.Text.Json;
using YSD.Client.Cli.Services.Abstractions;

namespace YSD.Client.Cli.Services;

public class CredsStorage : ICredsStorage
{
    private readonly string _tokensFilePath;
    private readonly string _appFolderPath;

    public CredsStorage()
    {
        _appFolderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "YSDTestTask");
        if (!Directory.Exists(_appFolderPath))
        {
            Directory.CreateDirectory(_appFolderPath);
        }
        
        _tokensFilePath = 
            Path.Combine(
                _appFolderPath,
                "auth_data");
    }

    public AccessRefreshToken? GetStoredTokens()
    {
        if (!File.Exists(_tokensFilePath)) return null;

        var fileData = File.ReadAllText(_tokensFilePath);

        return JsonSerializer.Deserialize<AccessRefreshToken>(fileData);
    }

    public void SaveTokens(string accessToken, string refreshToken)
    {
        var accessRefreshToken = new AccessRefreshToken(accessToken, refreshToken);

        var fileData = JsonSerializer.Serialize(accessRefreshToken);

        File.WriteAllText(_tokensFilePath, fileData);
    }
}