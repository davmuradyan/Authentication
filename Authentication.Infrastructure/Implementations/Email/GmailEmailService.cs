using Authentication.Application.Services.Email;
using Authentication.Infrastructure.Settings;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Authentication.Infrastructure.Implementations.Email;

public class GmailEmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public GmailEmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendAsync(EmailMessage message)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(message.To));
        email.Subject = message.Subject;

        email.Body = new TextPart("html")
        {
            Text = message.Body
        };

        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.AppPassword);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}