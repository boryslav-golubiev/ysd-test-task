using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using YSD.AuthenticationService.Application.Models;
using YSD.AuthenticationService.Application.Services.Abstractions;

namespace YSD.AuthenticationService.Application.Services;

public class UserInfoService(
    IUserClaimStore<IdentityUser> userStore) : IUserInfoService
{
    public async Task<UserInfo> GetUserInfoAsync(ClaimsPrincipal claims, CancellationToken cancellationToken = default)
    {
        var id = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value;

        var foundUser = await userStore.FindByIdAsync(id, cancellationToken);

        return new UserInfo(foundUser.Id, foundUser.NormalizedUserName);
    }
}