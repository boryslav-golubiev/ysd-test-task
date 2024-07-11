using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using YSD.AuthenticationService.Application.DependencyInjection;
using YSD.AuthenticationService.DAL;
using YSD.AuthenticationService.DAL.DependencyInjection;
using YSD.AuthenticationService.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services
    .AddControllers();

builder.Services
    .AddDal(configuration);

builder.Services
    .AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services
    .AddApplication();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]))
        };
    });

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