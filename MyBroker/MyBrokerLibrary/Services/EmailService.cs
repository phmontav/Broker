//using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MyBrokerLibrary.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;


namespace MyBrokerLibrary
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private EmailSettings _settings;
        public EmailService(IConfiguration config)
        {
            _config = config;

            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName;
            var path = projectDirectory + "\\MyBrokerLibrary\\EmailSettings.json";
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };
            try
            {
                _settings = JsonSerializer
                .Deserialize<EmailSettings>
                (
                    File.ReadAllText(path), options
                );
                //if (settings is null)
                //{
                //    throw new NullReferenceException("The specified language was not found in the json file.");
                //}
                
            }
            catch (Exception ex)
            {
                Console.WriteLine( ex.Message );
                //_log.LogError("Error looking up the custom text", ex);
                //throw;
            }

        }
        private string getApiKey()
        {
            return _config.GetSection("ConnectionString")["SendgridApiKey"];
        }

        public async Task sendEmail()
        {
            try
            {
                var client = new SendGridClient(getApiKey());
                var from = new EmailAddress(_settings.fromAddress);
                var subject = "Sending with SendGrid is Fun";
                var to = new EmailAddress(_settings.toEmail[0]);
                var plainTextContent = _settings.plainTextContent;
                var htmlContent = _settings.htmlContent;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                Console.WriteLine(response.StatusCode);
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
