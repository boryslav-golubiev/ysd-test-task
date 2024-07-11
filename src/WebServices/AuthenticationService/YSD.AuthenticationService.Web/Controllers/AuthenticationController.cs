using Microsoft.AspNetCore.Mvc;
using YSD.AuthenticationService.Application.Security.Abstractions;
using YSD.AuthenticationService.Application.Services.Abstractions;
using YSD.AuthenticationService.Web.DTOs;

namespace YSD.AuthenticationService.Web.Controllers;

[ApiController]
[Route("v1/authentication")]
public class AuthenticationController(
    IAuthenticationService authenticationService,
    ITokenService tokenService) : ControllerBase
{
    [HttpPost("auth")]
    public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateDto authenticateDto,
        CancellationToken cancellationToken)
    {
        var user = await authenticationService.AuthenticateUserAsync(
            authenticateDto.Login,
            authenticateDto.Password,
            cancellationToken);

        if (user is null) return Unauthorized("INVALID_CREDENTIALS");

        var accessRefreshToken = await tokenService.GenerateAccessRefreshTokenAsync(user, cancellationToken);

        return Ok(accessRefreshToken);
    }
}