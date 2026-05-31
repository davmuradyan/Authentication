namespace Authentication.Application.Services.Email;

public interface IEmailService
{
    Task SendAsync(EmailMessage message);
}