using Microsoft.AspNetCore.Identity;
using YSD.AuthenticationService.Application.Security.Abstractions;

namespace YSD.AuthenticationService.Application.Security;

public class AccessTokenStoreInMemory : IAccessTokenStore
{
    private const string AccessTokenPrefix = "accessToken_";
    private readonly IDictionary<string, string> _entries = new Dictionary<string, string>();

    public Task SaveAccessTokenAsync(string accessToken, IdentityUser user,
        CancellationToken cancellationToken = default)
    {
        var entryKey = CreateEntryKey(accessToken);
        _entries.TryAdd(entryKey, user.Id);
        return Task.CompletedTask;
    }

    public Task RemoveAccessTokenAsync(string accessToken,
        CancellationToken cancellationToken = default)
    {
        var entryKey = CreateEntryKey(accessToken);
        _entries.Remove(entryKey);
        return Task.CompletedTask;
    }

    public Task RemoveAccessTokenForUserAsync(IdentityUser user,
        CancellationToken cancellationToken = default)
    {
        var userKeys = _entries
            .Where(keyValue =>
                keyValue.Value == user.Id &&
                keyValue.Key.Contains(AccessTokenPrefix))
            .Select(keyValue => keyValue.Key)
            .ToList();

        foreach (var key in userKeys) _entries.Remove(key);

        return Task.CompletedTask;
    }

    public Task<bool> AccessTokenExistsAsync(string accessToken,
        CancellationToken cancellationToken = default)
    {
        var entryKey = CreateEntryKey(accessToken);
        return Task.FromResult(_entries.TryGetValue(entryKey, out var value));
    }

    private static string CreateEntryKey(string accessToken)
    {
        return $"{AccessTokenPrefix}{accessToken}";
    }
}