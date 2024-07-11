using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using YSD.AuthenticationService.Application.DependencyInjection;
using YSD.AuthenticationService.DAL;
using YSD.AuthenticationService.DAL.DependencyInjection;
using YSD.AuthenticationService.Web;
using YSD.AuthenticationService.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddUserSecrets<Program>();

var configuration = builder.Configuration;

builder.Services
    .AddControllers();

builder.Services
    .AddDal(configuration);

builder.Services
    .AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services
    .AddScoped<IUserPasswordStore<IdentityUser>>(serviceProvider =>
    {
        var userStore = serviceProvider.GetRequiredService<IUserStore<IdentityUser>>();
        return (IUserPasswordStore<IdentityUser>)userStore;
    });
builder.Services
    .AddScoped<IUserClaimStore<IdentityUser>>(serviceProvider =>
    {
        var userStore = serviceProvider.GetRequiredService<IUserStore<IdentityUser>>();
        return (IUserClaimStore<IdentityUser>)userStore;
    });

builder.Services
    .AddApplication();

builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        var validIssuer = configuration["Jwt:Issuer"]
                          ?? throw new ArgumentNullException("Jwt:Issuer");
        var secretKey = configuration["Jwt:SecretKey"]
                        ?? throw new ArgumentNullException("Jwt:SecretKey");
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey))
        };
    });
builder.Services
    .AddAuthorization();

builder.Services
    .AddHostedService<DefaultUserSeed>();

builder.Services.AddSingleton<
    IAuthorizationMiddlewareResultHandler, TokenValidationAuthorizationMiddlewareResultHandler>();

builder.Services
    .AddSwaggerGen();

var app = builder.Build();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger()
    .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); });

app.Run();