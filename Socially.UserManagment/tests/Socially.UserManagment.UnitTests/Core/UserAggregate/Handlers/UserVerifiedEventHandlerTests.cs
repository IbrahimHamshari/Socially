using Xunit;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.Core.Interfaces;
using Socially.UserManagment.Core.UserAggregate.Events;
using Socially.UserManagment.UseCases.Users.Handlers;

namespace Socially.UserManagment.UnitTests.Core.Users.Handlers;

public class UserVerifiedEventHandlerTests
{
  private readonly IEmailSender _emailSender;
  private readonly ILogger<UserVerifiedEventHandler> _logger;
  private readonly UserVerifiedEventHandler _handler;

  public UserVerifiedEventHandlerTests()
  {
    _emailSender = Substitute.For<IEmailSender>();
    _logger = Substitute.For<ILogger<UserVerifiedEventHandler>>();
    _handler = new UserVerifiedEventHandler(_emailSender, _logger);
  }

  // Test 1: Ensure that an email is sent when the user is verified
  [Fact]
  public async Task Handle_WhenUserVerified_ShouldSendVerificationEmail()
  {
    // Arrange
    var user = new User("validUser", "user@example.com", "Password@123", "John", "Doe", true);
    var notification = new UserVerifiedEvent(user);
    var cancellationToken = new CancellationToken();

    // Act
    await _handler.Handle(notification, cancellationToken);

    // Assert
    await _emailSender.Received(1).SendEmailAsync(
        user.Email,
        Arg.Is<string>(subject => subject == "Account Verification"),
        Arg.Is<string>(body => body.Contains($"Hi {user.Username}") &&
                               body.Contains("Congratulations! Your account has been successfully verified.") &&
                               body.Contains("You can now log in to your account and start using all the features of our platform"))
    );
  }

  // Test 2: Ensure that the logger logs the correct information
  [Fact]
  public async Task Handle_WhenUserVerified_ShouldLogInformation()
  {
    // Arrange
    var user = new User("validUser", "user@example.com", "Password@123", "John", "Doe", true);
    var notification = new UserVerifiedEvent(user);
    var cancellationToken = new CancellationToken();

    // Act
    await _handler.Handle(notification, cancellationToken);

    // Assert
    _logger.Received(1).Log(
        LogLevel.Information,
        Arg.Any<EventId>(),
        Arg.Is<object>(v => v.ToString()!.Contains($"a Register Email has been sent to the user with Id of {user.Id}")),
        null,
        Arg.Any<Func<object, Exception?, string>>());
  }
}
