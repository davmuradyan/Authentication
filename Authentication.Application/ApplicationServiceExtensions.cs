using Authentication.Application.Auth;
using Authentication.Application.CrudServices.Company;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddAuthApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICompanyService, CompanyService>();
        
        return services;
    }
}