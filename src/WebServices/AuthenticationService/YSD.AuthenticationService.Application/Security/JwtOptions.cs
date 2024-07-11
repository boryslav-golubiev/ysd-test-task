using System.ComponentModel.DataAnnotations;

namespace YSD.AuthenticationService.Application.Security;

public class JwtOptions
{
    [Required] public string SecretKey { get; init; } = null!;
    [Required] public string Issuer { get; init; } = null!;
    [Required] public int ExpirationHours { get; init; }
}