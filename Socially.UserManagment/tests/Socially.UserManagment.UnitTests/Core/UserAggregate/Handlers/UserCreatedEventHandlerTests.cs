using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Socially.ContentManagment.Core.Interfaces;
using Socially.ContentManagment.Core.UserAggregate;
using Socially.ContentManagment.Core.UserAggregate.Events;
using Socially.ContentManagment.UseCases.Users.Handlers;
using Xunit;

namespace Socially.ContentManagment.UnitTests.Core.Users.Handlers;

public class UserCreatedEventHandlerTests
{
  private readonly IEmailSender _emailSender;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly ILogger<UserCreatedEventHandler> _logger;
  private readonly UserCreatedEventHandler _handler;

  public UserCreatedEventHandlerTests()
  {
    _emailSender = Substitute.For<IEmailSender>();
    _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    _logger = Substitute.For<ILogger<UserCreatedEventHandler>>();
    _handler = new UserCreatedEventHandler(_emailSender, _httpContextAccessor, _logger);
  }

  // Test 1: Ensure that the welcome email is sent with the verification link
  [Fact]
  public async Task Handle_WhenUserCreated_ShouldSendWelcomeEmailWithVerificationLink()
  {
    // Arrange
    var user = new User(Guid.NewGuid(), "validUser", "user@example.com", "Password@123", "John", "Doe", true);
    user.GenerateEmailVerificationToken(); // Assuming this sets VerificationToken
    var notification = new UserCreatedEvent(user);
    var cancellationToken = new CancellationToken();

    // Mock HttpContext to include the request host information
    var mockHttpContext = new DefaultHttpContext();
    mockHttpContext.Request.Host = new HostString("www.example.com");
    _httpContextAccessor.HttpContext.Returns(mockHttpContext);

    // Act
    await _handler.Handle(notification, cancellationToken);

    // Assert
    await _emailSender.Received(1).SendEmailAsync(
        user.Email,
        Arg.Is<string>(subject => subject == "Account Registeration"),
        Arg.Is<string>(body => body.Contains($"Hi {user.Username}") &&
                               body.Contains(user.VerificationToken!) &&
                               body.Contains("https://www.example.com"))
    );
  }

  // Test 2: Ensure that the logger logs the correct information
  [Fact]
  public async Task Handle_WhenUserCreated_ShouldLogInformation()
  {
    // Arrange
    var user = new User(Guid.NewGuid(), "validUser", "user@example.com", "Password@123", "John", "Doe", true);
    user.GenerateEmailVerificationToken(); // Assuming this sets VerificationToken
    var notification = new UserCreatedEvent(user);
    var cancellationToken = new CancellationToken();

    // Mock HttpContext
    var mockHttpContext = new DefaultHttpContext();
    mockHttpContext.Request.Host = new HostString("www.example.com");
    _httpContextAccessor.HttpContext.Returns(mockHttpContext);

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
