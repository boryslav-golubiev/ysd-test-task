using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YSD.AuthenticationService.DAL.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddDal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MySQL");
        var serverVersion = configuration.GetConnectionString("MySQL_ServerVersion");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySql(connectionString, new MySqlServerVersion(serverVersion));
        });
    }
}