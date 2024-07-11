using Microsoft.AspNetCore.Mvc;
using YSD.AuthenticationService.Application.Security.Abstractions;
using YSD.AuthenticationService.Web.DTOs;

namespace YSD.AuthenticationService.Web.Controllers;

[ApiController]
[Route("v1/token")]
public class TokenController(ITokenService tokenService) : ControllerBase
{
    [HttpPost("validate")]
    public async Task<IActionResult> ValidateTokenAsync([FromBody] ValidateTokenDto validateTokenDto,
        CancellationToken cancellationToken)
    {
        var validationResult =
            await tokenService.ValidateAccessTokenAsync(validateTokenDto.AccessToken, cancellationToken);
        if (!validationResult) return Unauthorized();

        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDto refreshTokenDto,
        CancellationToken cancellationToken)
    {
        var accessRefreshToken =
            await tokenService.RefreshTokenAsync(refreshTokenDto.RefreshToken, cancellationToken);
        if (accessRefreshToken is null) return Unauthorized();

        return Ok(accessRefreshToken);
    }
}