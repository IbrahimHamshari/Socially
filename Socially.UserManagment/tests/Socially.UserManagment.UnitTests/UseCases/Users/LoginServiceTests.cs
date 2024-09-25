using Ardalis.Result;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Socially.UserManagment.Core.RefreshTokenAggregate;
using Socially.UserManagment.Core.UserAggregate;
using Socially.UserManagment.UseCases.Users.Interfaces;
using Socially.UserManagment.UseCases.Users.Login;
using Xunit;

namespace Socially.UserManagment.UnitTests.UseCases.Users;

public class LoginServiceTests
{
  private readonly ITokenGenerator _tokenGenerator;
  private readonly ILogger<LoginService> _logger;
  private readonly LoginService _loginService;

  public LoginServiceTests()
  {
    _tokenGenerator = Substitute.For<ITokenGenerator>();
    _logger = Substitute.For<ILogger<LoginService>>();
    _loginService = new LoginService(_tokenGenerator, _logger);
  }

  // Test 1: Ensure that a user with the correct password logs in successfully and tokens are generated
  [Fact]
  public async Task LoginAsync_WithValidPassword_ShouldGenerateTokensAndLogLogin()
  {
    // Arrange
    var user = new User("validUser", "user@example.com", "Password@123", "John", "Doe", true);
    var password = "Password@123";

    // Mock token generation
    _tokenGenerator.GenerateAccessToken(user.Id).Returns("access_token");
    _tokenGenerator.GenerateRefreshToken(user.Id).Returns(Task.FromResult(new RefreshToken(user.Id, "refresh_token", DateTimeOffset.UtcNow.AddDays(1), null, "family")));

    // Act
    var result = await _loginService.LoginAsync(user, password);

    // Assert
    result.IsSuccess.Should().BeTrue();
    result.Value.AccessToken.Should().Be("access_token");
    result.Value.RefreshToken.Should().Be("refresh_token");

    // Verify that the login is logged
    _logger.Received(1).Log(
        LogLevel.Information,
        Arg.Any<EventId>(),
        Arg.Is<object>(v => v.ToString()!.Contains($"User with ID of {user.Id} has logged In")),
        null,
        Arg.Any<Func<object, Exception?, string>>());
  }

  // Test 2: Ensure that if the password is incorrect, login fails with unauthorized result
  [Fact]
  public async Task LoginAsync_WithInvalidPassword_ShouldReturnUnauthorized()
  {
    // Arrange
    var user = new User("validUser", "user@example.com", "Password@123", "John", "Doe", true);
    var password = "WrongPassword@123";

    // Act
    var result = await _loginService.LoginAsync(user, password);

    // Assert
    result.Status.Should().Be(ResultStatus.Unauthorized);

    // Verify that no tokens are generated
    _tokenGenerator.DidNotReceive().GenerateAccessToken(Arg.Any<Guid>());
    await _tokenGenerator.DidNotReceive().GenerateRefreshToken(Arg.Any<Guid>());

    // Ensure no login is logged - no need to use argument matchers for `DidNotReceive`
    _logger.DidNotReceive().LogInformation(default(string?), Array.Empty<object>());
  }

  // Test 3: Ensure that if the user is inactive, login fails with unauthorized result
  [Fact]
  public async Task LoginAsync_WithInactiveUser_ShouldReturnUnauthorized()
  {
    // Arrange
    var user = new User("validUser", "user@example.com", "Password@123", "John", "Doe", true);
    var password = "Password@123";
    user.DeactivateAccount();

    // Act
    var result = await _loginService.LoginAsync(user, password);

    // Assert
    result.Status.Should().Be(ResultStatus.Unauthorized);

    // Verify that no tokens are generated
    _tokenGenerator.DidNotReceive().GenerateAccessToken(Arg.Any<Guid>());
    await _tokenGenerator.DidNotReceive().GenerateRefreshToken(Arg.Any<Guid>());

    // Ensure no login is logged
    _logger.DidNotReceive().LogInformation(default(string?), Array.Empty<object>());
  }
}
