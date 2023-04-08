//using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyBrokerLibrary.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;


namespace MyBrokerLibrary
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration config;
        private readonly ILogger<EmailService> logger;
        private EmailSettings settings;
        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            this.config = config;
            this.logger = logger;
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName;
            var path = projectDirectory + "\\MyBrokerLibrary\\EmailSettings.json";
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };
            try
            {
                this.settings = JsonSerializer
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
                this.logger.LogError("error when reading email settings file", ex);
                throw;
            }

        }
        private string getApiKey()
        {

            try
            {
                return this.config.GetSection("ConnectionString")["SendgridApiKey"];
            }
            catch (Exception) { throw; }
        }

        public (string subject, string plainTextContent, string htmlContent) messageContent(string action, string ticker, string price) 
        {
            string subject = "", plainTextContent = "", htmlContent = "";
            if (action.ToLower() == "buy")
            {
                subject = "Buy Action Needed";
                plainTextContent = $"The Price of {ticker} has dropped to {price}, buy it now!";
                htmlContent = "<strong>" + plainTextContent + "</strong>";
            }
            else
            {
                subject = "Sell Action Needed";
                plainTextContent = $"The Price of {ticker} has risen to {price}, sell it now!";
                htmlContent = "<strong>" + plainTextContent + "</strong>";
            }
            return (subject, plainTextContent, htmlContent);
        }    

        public async Task sendEmail(string ticker, string action, string price)
        {
            try
            {
                var client = new SendGridClient(getApiKey());
                var from = new EmailAddress(this.settings.fromAddress);
                (string subject, string plainTextContent, string htmlContent) = messageContent(action, ticker, price);  
                foreach(var targetEmail in this.settings.toEmail.ToList())
                {
                    var msg = MailHelper.CreateSingleEmail(from,new EmailAddress(targetEmail), subject, plainTextContent, htmlContent);
                    var response = await client.SendEmailAsync(msg);
                    Console.WriteLine(response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error sending emails", ex);
                throw;
            }

        }

    }
}
