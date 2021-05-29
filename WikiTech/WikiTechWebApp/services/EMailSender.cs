using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using WikiTechWebApp.services;

namespace WikiTechWebApp.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var SendGridKey = "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";

            //return Execute(Options.SendGridKey, subject, message, email);
            return Execute(SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("loanrey2000@gmail.com", "No-reply@wikitech.ch"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            //var result = client.SendEmailAsync(msg);
            return client.SendEmailAsync(msg);
        }
    }
}