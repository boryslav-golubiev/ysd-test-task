using System.Security.Claims;
using YSD.AuthenticationService.Application.Models;

namespace YSD.AuthenticationService.Application.Services.Abstractions;

public interface IUserInfoService
{
    Task<UserInfo> GetUserInfoAsync(ClaimsPrincipal claims, CancellationToken cancellationToken = default);
}