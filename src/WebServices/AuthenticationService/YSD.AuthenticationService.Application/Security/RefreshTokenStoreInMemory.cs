using Microsoft.AspNetCore.Identity;
using YSD.AuthenticationService.Application.Security.Abstractions;

namespace YSD.AuthenticationService.Application.Security;

public class RefreshTokenStoreInMemory(IUserStore<IdentityUser> userStore) : IRefreshTokenStore
{
    private const string RefreshTokenPrefix = "refreshToken_";
    private readonly IDictionary<string, string> _entries = new Dictionary<string, string>();

    public Task SaveRefreshTokenAsync(string refreshToken, IdentityUser user,
        CancellationToken cancellationToken = default)
    {
        var entryKey = CreateEntryKey(refreshToken);
        _entries.Add(entryKey, user.Id);
        return Task.CompletedTask;
    }

    public Task RemoveRefreshTokenAsync(string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var entryKey = CreateEntryKey(refreshToken);
        _entries.Remove(entryKey);
        return Task.CompletedTask;
    }

    public Task RemoveRefreshTokenForUserAsync(IdentityUser user,
        CancellationToken cancellationToken = default)
    {
        var userKeys = _entries
            .Where(keyValue =>
                keyValue.Value == user.Id &&
                keyValue.Key.Contains(RefreshTokenPrefix))
            .Select(keyValue => keyValue.Key)
            .ToList();

        foreach (var key in userKeys) _entries.Remove(key);

        return Task.CompletedTask;
    }

    public async Task<IdentityUser?> GetRefreshTokenUserAsync(string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var entryKey = CreateEntryKey(refreshToken);
        var isFound = _entries.TryGetValue(entryKey, out var userId);

        if (!isFound) return null;

        return await userStore.FindByIdAsync(userId, cancellationToken);
    }

    private static string CreateEntryKey(string refreshToken)
    {
        return $"{RefreshTokenPrefix}{refreshToken}";
    }
}