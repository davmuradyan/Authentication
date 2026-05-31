using Authentication.Application.Services.Email;
using Authentication.Application.Services.HashFunctions;
using Authentication.Application.Services.Repositories;
using Authentication.Application.Services.Tokens;
using Authentication.Infrastructure.Implementations.Email;
using Authentication.Infrastructure.Implementations.HashFunctions;
using Authentication.Infrastructure.Implementations.Repositories;
using Authentication.Infrastructure.Implementations.Tokens;
using Authentication.Infrastructure.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
    
        // Tokens & Hashing
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordHashService, BCryptHashService>();
        services.AddScoped<ITokenHashService, SHA256HashService>();
    
        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IUserActivationTokenRepository, UserActivationTokenRepository>();
        services.AddScoped<ICompanyUserRepository, CompanyUserRepository>();
    
        // Email
        services.AddScoped<IEmailTemplateService>(_ => new EmailTemplateService(env));
        services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
        services.AddScoped<IEmailService, GmailEmailService>();
        
        services.AddHttpContextAccessor();

        // Authorization Policies
        services.AddAuthorization(options =>
        {
            options.AddPolicy($"{nameof(Policies.Company.Create)}", 
                policy => policy.RequireAssertion(context =>
                {
                    // Check if user has global-admin role
                    var isGlobalAdmin = context.User.HasClaim(
                        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", 
                        "global-admin");
                    
                    // Check if user has Company.Create permission
                    var hasPermission = context.User.HasClaim("permission", "Company.Create");
                    
                    return isGlobalAdmin || hasPermission;
                }));
        });

        return services;
    }
}