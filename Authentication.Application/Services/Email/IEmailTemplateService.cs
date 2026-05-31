namespace Authentication.Application.Services.Email;

public interface IEmailTemplateService
{
    string GetActivationEmailBody(string activationLink);
}