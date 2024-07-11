using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YSD.AuthenticationService.Application.Services.Abstractions;
using YSD.AuthenticationService.Web.DTOs;

namespace YSD.AuthenticationService.Web.Controllers;

[ApiController]
[Route("v1/userInfo")]
public class UserInfoController(
    IUserInfoService userInfoService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserInfoAsync(CancellationToken cancellationToken)
    {
        var userInfo = await userInfoService.GetUserInfoAsync(User, cancellationToken);

        var responseDto = new UserInfoDto(userInfo.Id, userInfo.Login);

        return Ok(responseDto);
    }
}