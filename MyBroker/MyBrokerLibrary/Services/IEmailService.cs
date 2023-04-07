//using Microsoft.Extensions.Configuration;
namespace MyBrokerLibrary
{
    public interface IEmailService
    {
        Task sendEmail();
    }
}