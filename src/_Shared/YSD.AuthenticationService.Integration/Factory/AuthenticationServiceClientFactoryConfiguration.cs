using System.ComponentModel.DataAnnotations;

namespace YSD.AuthenticationService.Integration.Factory;

public class AuthenticationServiceClientFactoryConfiguration
{
    [Required] public Uri BaseUrl { get; set; }
}