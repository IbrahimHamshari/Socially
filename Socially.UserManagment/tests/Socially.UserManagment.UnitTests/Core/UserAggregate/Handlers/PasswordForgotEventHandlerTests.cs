using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Socially.ContentManagment.Core.Interfaces;
using Socially.ContentManagment.Core.UserAggregate;
using Socially.ContentManagment.UseCases.Users.ForgetPassword;
using Socially.ContentManagment.UseCases.Users.Handlers;
using Xunit;

namespace Socially.ContentManagment.UnitTests.Core.Users.Handlers;

public class PasswordForgotEventHandlerTests
{
  private readonly IEmailSender _emailSender;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly ILogger<PasswordForgotEventHandler> _logger;
  private readonly PasswordForgotEventHandler _handler;

  public PasswordForgotEventHandlerTests()
  {
    _emailSender = Substitute.For<IEmailSender>();
    _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    _logger = Substitute.For<ILogger<PasswordForgotEventHandler>>();
    _handler = new PasswordForgotEventHandler(_emailSender, _httpContextAccessor, _logger);
  }

  // Test 1: Ensure that the email is sent when the password is forgotten
  [Fact]
  public async Task Handle_WhenPasswordForgot_ShouldSendEmailWithResetLink()
  {
    // Arrange
    var user = new User(Guid.NewGuid(), "validUser", "user@example.com", "Password@123", "John", "Doe", true);
    user.GenerateResetToken(); // Assuming this sets ResetPasswordToken
    var notification = new PasswordForgotEvent(user);
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
        Arg.Is<string>(subject => subject == "Reset Account Password"),
        Arg.Is<string>(body => body.Contains("Hi validUser") &&
                                body.Contains(user.ResetPasswordToken!) &&
                                body.Contains("https://www.example.com"))
    );
  }

  // Test 2: Ensure that the logger logs the correct information
  [Fact]
  public async Task Handle_WhenPasswordForgot_ShouldLogInformation()
  {
    // Arrange
    var user = new User(Guid.NewGuid(), "validUser", "user@example.com", "Password@123", "John", "Doe", true);
    user.GenerateResetToken();
    var notification = new PasswordForgotEvent(user);
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
        Arg.Is<object>(v => v.ToString()!.Contains($"Sucessfully Changed Password to the user with id of {user.Id}")),
        null,
        Arg.Any<Func<object, Exception?, string>>());
  }
}
