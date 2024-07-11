using System.ComponentModel.DataAnnotations;

namespace YSD.AuthenticationService.Web.DTOs;

public record AuthenticateDto([Required] string Login, [Required] string Password);