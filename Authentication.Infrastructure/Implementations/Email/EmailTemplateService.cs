using Authentication.Application.Services.Email;
using Microsoft.AspNetCore.Hosting;

namespace Authentication.Infrastructure.Implementations.Email;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly string _templatesPath;

    public EmailTemplateService(IWebHostEnvironment env)
    {
        _templatesPath = Path.Combine(env.ContentRootPath, "Templates", "Email");
    }

    public string GetActivationEmailBody(string activationLink)
    {
        var templatePath = Path.Combine(_templatesPath, "ActivationEmail.html");
        var template = File.ReadAllText(templatePath);
        return template.Replace("{{ACTIVATION_LINK}}", activationLink);
    }
}