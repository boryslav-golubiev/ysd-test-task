using System.ComponentModel.DataAnnotations;

namespace YSD.AuthenticationService.Web.DTOs;

public record ValidateTokenDto([Required] string AccessToken);