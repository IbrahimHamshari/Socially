using Microsoft.Extensions.Logging;
using NSubstitute;
using Socially.UserManagment.Core.Interfaces;
using Socially.UserManagment.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Events;
using Socially.UserManagment.UseCases.Users.Handlers;
using Xunit;

namespace Socially.UserManagment.UnitTests.Core.Users.Handlers;

public class RecoverAccountEventHandlerTests
{
  private readonly IEmailSender _emailSender;
  private readonly ILogger<RecoverAccountEventHandler> _logger;
  private readonly RecoverAccountEventHandler _handler;

  public RecoverAccountEventHandlerTests()
  {
    _emailSender = Substitute.For<IEmailSender>();
    _logger = Substitute.For<ILogger<RecoverAccountEventHandler>>();
    _handler = new RecoverAccountEventHandler(_emailSender, _logger);
  }

  // Test 1: Ensure that the email is sent when the account is recovered
  [Fact]
  public async Task Handle_WhenAccountRecovered_ShouldSendEmail()
  {
    // Arrange
    var user = new User(Guid.NewGuid(),"validUser", "user@example.com", "Password@123", "John", "Doe", true);
    var notification = new AccountRecoveredEvent(user);
    var cancellationToken = new CancellationToken();

    // Act
    await _handler.Handle(notification, cancellationToken);

    // Assert
    await _emailSender.Received(1).SendEmailAsync(
        user.Email,
        Arg.Is<string>(subject => subject == "Sucssefully changing the password"),
        Arg.Is<string>(body => body.Contains("Hi validUser") && body.Contains("This is a confirmation that your password was successfully changed"))
    );
  }

  // Test 2: Ensure that the logger logs the correct information
  [Fact]
  public async Task Handle_WhenAccountRecovered_ShouldLogInformation()
  {
    // Arrange
    var user = new User(Guid.NewGuid(),"validUser", "user@example.com", "Password@123", "John", "Doe", true);
    var notification = new AccountRecoveredEvent(user);
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
