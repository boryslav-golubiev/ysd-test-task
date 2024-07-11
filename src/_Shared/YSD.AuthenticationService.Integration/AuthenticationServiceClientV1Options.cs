using System.ComponentModel.DataAnnotations;

namespace YSD.AuthenticationService.Integration;

public record AuthenticationServiceClientV1Options([Required] Uri BaseUrl);