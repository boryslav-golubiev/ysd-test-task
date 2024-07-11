using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using YSD.AuthenticationService.Application.Security.Abstractions;

namespace YSD.AuthenticationService.Web.Middleware;

public class TokenValidationAuthorizationMiddlewareResultHandler
    : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Succeeded)
        {
            var tokenService = context.RequestServices.GetRequiredService<ITokenService>();
            var authHeader = context.Request.Headers.Authorization.ToString();
            var token = authHeader.Replace("Bearer ", string.Empty);
            var validationResult = await tokenService.ValidateAccessTokenAsync(token, context.RequestAborted);
            if (!validationResult)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}