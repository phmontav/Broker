//using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyBrokerLibrary.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MyBrokerLibrary
{
    public class EmailService : IEmailService
    {
        private EmailSettings _settings;
        public EmailService()
        {
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                EmailSettings _settings = JsonSerializer
                .Deserialize<EmailSettings>
                (
                    File.ReadAllText("EmailOptions.json"), options
                );

                //if (settings is null)
                //{
                //    throw new NullReferenceException("The specified language was not found in the json file.");
                //}
                
            }
            catch (Exception ex)
            {
                //_log.LogError("Error looking up the custom text", ex);
                //throw;
            }
        }

    }
}
