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

public class UserChangedPasswordEventHandlerTests
{
  private readonly IEmailSender _emailSender;
  private readonly ILogger<UserChangedPasswordEventHandler> _logger;
  private readonly UserChangedPasswordEventHandler _handler;

  public UserChangedPasswordEventHandlerTests()
  {
    _emailSender = Substitute.For<IEmailSender>();
    _logger = Substitute.For<ILogger<UserChangedPasswordEventHandler>>();
    _handler = new UserChangedPasswordEventHandler(_emailSender, _logger);
  }

  // Test 1: Ensure that an email is sent when the user changes their password
  [Fact]
  public async Task Handle_WhenPasswordChanged_ShouldSendEmail()
  {
    // Arrange
    var user = new User("validUser", "user@example.com", "Password@123", "John", "Doe", true);
    var notification = new UserChangedPasswordEvent(user);
    var cancellationToken = new CancellationToken();
    var changeTime = DateTimeOffset.UtcNow;

    // Act
    await _handler.Handle(notification, cancellationToken);

    // Assert
    await _emailSender.Received(1).SendEmailAsync(
        user.Email,
        Arg.Is<string>(subject => subject == "Password Changed"),
        Arg.Is<string>(body => body.Contains($"Hi {user.Username}") &&
                               body.Contains($"Your password was successfully changed on {changeTime:MMMM dd, yyyy}") &&
                               body.Contains("If you did not make this change, please contact our support team immediately"))
    );
  }

  // Test 2: Ensure that the logger logs the correct information
  [Fact]
  public async Task Handle_WhenPasswordChanged_ShouldLogInformation()
  {
    // Arrange
    var user = new User("validUser", "user@example.com", "Password@123", "John", "Doe", true);
    var notification = new UserChangedPasswordEvent(user);
    var cancellationToken = new CancellationToken();

    // Act
    await _handler.Handle(notification, cancellationToken);

    // Assert
    _logger.Received(1).Log(
        LogLevel.Information,
        Arg.Any<EventId>(),
        Arg.Is<object>(v => v.ToString()!.Contains($"Sucessfully Changed Password to the user with id of {user.Id}")),
        null,
        Arg.Any<Func<object, Exception?, string>>());
  }
}
