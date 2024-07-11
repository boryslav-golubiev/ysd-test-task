using System.ComponentModel.DataAnnotations;

namespace YSD.AuthenticationService.Integration.Factory;

public record AuthenticationServiceClientFactoryConfiguration([Required] Uri BaseUrl);