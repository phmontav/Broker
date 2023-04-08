//using Microsoft.Extensions.Configuration;
namespace MyBrokerLibrary
{
    public interface IEmailService
    {
        Task sendEmail(string ticker, string action, string price);
    }
}