using Cocona;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using YSD.AuthenticationService.Integration.DependencyInjection;
using YSD.Client.Cli;
using YSD.Client.Cli.Services;
using YSD.Client.Cli.Services.Abstractions;

var coconaBuilder = CoconaApp
    .CreateBuilder(args);

var logger = new LoggerConfiguration()
    .WriteTo.File(
        "log.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

coconaBuilder.Logging.ClearProviders();
coconaBuilder.Logging.AddSerilog(logger);

coconaBuilder.Configuration
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>();

coconaBuilder.Services
    .AddSingleton<ICredsStorage, CredsStorage>();

coconaBuilder.Services
    .AddAuthenticationServiceClientV1();

var app = coconaBuilder.Build();

await app.RunAsync<Commands>();