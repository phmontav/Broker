//using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyBrokerLibrary.Models;
using System.Net.Mail;
using System.Net;

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
        public (string subject, string plainTextContent) messageContent(string ticker, string action, string price) 
        {
            string subject = "", plainTextContent = "", htmlContent = "";
            if (action.ToLower() == "buy")
            {
                subject = "Buy Action Needed";
                plainTextContent = $"The Price of {ticker} has dropped to {price}, buy it now!";
            }
            else
            {
                subject = "Sell Action Needed";
                plainTextContent = $"The Price of {ticker} has risen to {price}, sell it now!";
            }
            return (subject, plainTextContent);
        }    

        public async Task sendEmail(string ticker, string action, string price)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(this.settings.smtp.host, int.Parse(this.settings.smtp.port));
                smtpClient.Credentials = new NetworkCredential(this.settings.smtp.username, this.settings.smtp.password);
                smtpClient.EnableSsl = bool.Parse(this.settings.smtp.ssl);
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(this.settings.smtp.username);
                (mailMessage.Subject, mailMessage.Body) = messageContent(ticker, action, price);
                foreach (var targetEmail in this.settings.toEmail.ToList())
                {
                    mailMessage.To.Add(new MailAddress(targetEmail));
                }
                 await smtpClient.SendMailAsync(mailMessage);
                foreach(var targetEmail in mailMessage.To)
                {
                    this.logger.LogInformation("Email sent to {targetEmail}", targetEmail);
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
