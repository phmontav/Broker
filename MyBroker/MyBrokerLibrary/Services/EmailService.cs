using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MyBroker
{
    public class EmailService
    {
        private readonly IConfigurationRoot _config;

        EmailService(IConfigurationRoot config)
        {
            _config = config;
        }
        static async Task sendEmail()
        {
            IConfigurationSection options = _config.GetSection("SMTPSettings");
            var apiKey = _config.GetSection("ConnectionString")["SMTPApiKey"];
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(options["fromAddress"],"example user");
            var to = new EmailAddress(options["toEmail"], "Example User");
            var subject = options["subject"];
            var plainTextContent = options["plainTextContent"];
            var htmlContent = options["htmlContent"];
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);
            Console.WriteLine(response.StatusCode);

        }
    }
}
