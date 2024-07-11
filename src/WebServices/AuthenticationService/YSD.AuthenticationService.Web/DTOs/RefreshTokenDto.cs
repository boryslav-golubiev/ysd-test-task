using System.ComponentModel.DataAnnotations;

namespace YSD.AuthenticationService.Web.DTOs;

public record RefreshTokenDto([Required] string RefreshToken);