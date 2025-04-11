using BugTracker.Application.Settings;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BugTracker.Application.Services.Implementation.Auth
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSetting;

        public EmailSender(IOptions<EmailSettings> options)
        {
            _emailSetting = options.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_emailSetting.Email),
                Subject = subject
            };
            message.To.Add(MailboxAddress.Parse(email));

            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };
            message.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_emailSetting.Host, _emailSetting.port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSetting.Email, _emailSetting.Password);
            await smtp.SendAsync(message);
            smtp.Disconnect(true);

        }
    }
}
