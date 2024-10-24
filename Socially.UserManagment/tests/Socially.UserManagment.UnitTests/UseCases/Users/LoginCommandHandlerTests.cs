using Ardalis.Result;
using Ardalis.SharedKernel;
using FluentAssertions;
using NSubstitute;
using Socially.ContentManagment.Core.UserAggregate;
using Socially.ContentManagment.Core.UserAggregate.Specifications;
using Socially.ContentManagment.UseCases.Users.Common;
using Socially.ContentManagment.UseCases.Users.Common.DTOs;
using Socially.ContentManagment.UseCases.Users.Login;
using Xunit;

namespace Socially.ContentManagment.UnitTests.UseCases.Users;

public class LoginCommandHandlerTests
{
  private readonly IRepository<User> _repository;
  private readonly ILoginService _loginService;
  private readonly LoginCommandHandler _handler;

  public LoginCommandHandlerTests()
  {
    _repository = Substitute.For<IRepository<User>>();
    _loginService = Substitute.For<ILoginService>();
    _handler = new LoginCommandHandler(_repository, _loginService);
  }

  // Test 1: Ensure that if the user is found by username, the login process works and tokens are returned
  [Fact]
  public async Task Handle_WithValidUsername_ShouldLoginAndReturnTokens()
  {
    // Arrange
    var user = new User(Guid.NewGuid(), "validUser", "user@example.com", "Password@123", "John", "Doe", true);
    var command = new LoginCommand(new UserLoginDto { Username = "validUser", Password = "Password@123" });

    // Mock repository to return the user for the valid username
    _repository.FirstOrDefaultAsync(Arg.Any<UserByUsernameSpec>(), Arg.Any<CancellationToken>())
        .Returns(user);

    // Mock successful login result
    var tokens = new Tokens { AccessToken = "access_token", RefreshToken = "refresh_token" };
    _loginService.LoginAsync(user, command.User.Password).Returns(tokens);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsSuccess.Should().BeTrue();
    result.Value.Should().Be(tokens);

    // Verify that the login service was called with the correct user and password
    await _loginService.Received(1).LoginAsync(user, command.User.Password);
  }

  // Test 2: Ensure that if the user is not found, the proper error is returned
  [Fact]
  public async Task Handle_WithInvalidUsername_ShouldReturnNotFoundError()
  {
    // Arrange
    var command = new LoginCommand(new UserLoginDto { Username = "nonexistentUser", Password = "Password123" });

    // Mock repository to return null for the non-existent username
    _repository.FirstOrDefaultAsync(Arg.Any<UserByUsernameSpec>(), Arg.Any<CancellationToken>())
        .Returns((User?)null);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Status.Should().Be(ResultStatus.NotFound);
    result.Errors.Should().ContainSingle(e => e == $"The user with Username = '{command.User.Username}' was not found");

    // Verify that the login service was not called
    await _loginService.DidNotReceive().LoginAsync(Arg.Any<User>(), Arg.Any<string>());
  }
}
