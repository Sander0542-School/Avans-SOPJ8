using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Bumbo.Logic.Options;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace Bumbo.Web
{
    public class EmailSender : IEmailSender
    {
        private readonly MailOptions _options;

        public EmailSender(IOptions<MailOptions> mailOptions)
        {
            _options = mailOptions.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_options.Host, _options.Port)
            {
                Credentials = new NetworkCredential(_options.UserName, _options.Password),
                EnableSsl = _options.UseTls
            };

            await client.SendMailAsync($"{_options.FromName} <{_options.FromEmail}>", email, subject, htmlMessage);
        }
    }
}
