using System;
using System.IO;
using Castle.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Moq;
using MyBrokerLibrary;
using MyBrokerLibrary.Models;
using Xunit;

namespace MyBrokerTests.Services
{
    public class EmailServiceTests
    {
        [Fact]
        public void Test_SendEmail_Success()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<EmailService>>();
            var emailService = new EmailService(loggerMock.Object);
            var ticker = "PETR4";
            var action = "buy";
            var price = "100.00";

            // Act
            emailService.sendEmail(ticker, action, price);

            // Assert
            // Verify that the logger was called with the expected log level and message
            loggerMock.Verify(x => x.Log(
                            It.Is<LogLevel>(l => l == LogLevel.Information),
                            It.IsAny<EventId>(),
                            It.IsAny<It.IsAnyType>(),
                            It.IsAny<Exception>(),
                            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));

        }
        [Fact]
        public void Test_SendEmail_Failure()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<EmailService>>();
            var emailService = new EmailService(loggerMock.Object);
            var ticker = "PETR4";
            var action = "buy";
            var price = "100.00";

            // Act
            emailService.sendEmail(ticker, action, price);

            // Assert
            // Verify that the logger was called with the expected log level and message
            loggerMock.Verify(x => x.Log(
                            It.Is<LogLevel>(l => l == LogLevel.Critical),
                            It.IsAny<EventId>(),
                            It.IsAny<It.IsAnyType>(),
                            It.IsAny<Exception>(),
                            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));

        }
        [Fact]
        public void MessageContent_WithBuyAction_ShouldReturnCorrectMessage()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<EmailService>>();
            var emailService = new EmailService(loggerMock.Object);
            var ticker = "PETR4";
            var action = "buy";
            var price = "100.00";
            var expectedSubject = "Buy Action Needed";
            var expectedPlainTextContent = "The Price of PETR4 has dropped to 100.00, buy it now!";

            // Act
            var (subject, plainTextContent) = emailService.messageContent(ticker, action, price);

            // Assert
            Assert.Equal(expectedSubject, subject);
            Assert.Equal(expectedPlainTextContent, plainTextContent);
        }
        [Fact]
        public void MessageContent_WithSellAction_ShouldReturnCorrectMessage()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<EmailService>>();
            var emailService = new EmailService(loggerMock.Object);
            var ticker = "PETR4";
            var action = "sell";
            var price = "100.00";
            var expectedSubject = "Sell Action Needed";
            var expectedPlainTextContent = "The Price of PETR4 has risen to 100.00, sell it now!";

            // Act
            var (subject, plainTextContent) = emailService.messageContent(ticker, action, price);

            // Assert
            Assert.Equal(expectedSubject, subject);
            Assert.Equal(expectedPlainTextContent, plainTextContent);
        }
    }
}
