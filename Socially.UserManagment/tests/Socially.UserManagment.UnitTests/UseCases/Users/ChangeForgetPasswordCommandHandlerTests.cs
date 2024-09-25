using Xunit;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ardalis.Result;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Errors;
using Socially.UserManagment.Core.UserAggregate.Specifications;
using Socially.UserManagment.UseCases.Users.ChangePassword;
using Ardalis.SharedKernel;
using FluentAssertions;
using Socially.UserManagment.UseCases.Users.Common.DTOs;
using Socially.UserManagment.UseCases.Users.Login;
using Socially.UserManagment.UseCases.Users.ChangeForgetPassword;
using Socially.UserManagment.UseCases.Users.ChangePasswordForget;
using Socially.UserManagment.UseCases.Users.Common;
namespace Socially.UserManagment.UnitTests.UseCases.Users;

public class ChangeForgetPasswordCommandHandlerTests
{
  private readonly IRepository<User> _repository;
  private readonly ILoginService _loginService;
  private readonly ChangeForgetPasswordCommandHandler _handler;

  public ChangeForgetPasswordCommandHandlerTests()
  {
    _repository = Substitute.For<IRepository<User>>();
    _loginService = Substitute.For<ILoginService>();
    _handler = new ChangeForgetPasswordCommandHandler(_repository, _loginService);
  }

  // Test 1: Ensure that if the user is not found by reset token, the proper error is returned
  [Fact]
  public async Task Handle_WithInvalidToken_ShouldReturnNotFoundError()
  {
    // Arrange
    var command = new ChangeForgetPasswordCommand ("invalid-token","NewPassword123" );
    _repository.SingleOrDefaultAsync(Arg.Any<UserByResetTokenSpec>(), Arg.Any<CancellationToken>())
        .Returns((User?)null);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Status.Should().Be(ResultStatus.NotFound);
    result.IsNotFound();
  }

  // Test 2: Ensure that a user can successfully change their password and login
  [Fact]
  public async Task Handle_WithValidToken_ShouldChangePasswordAndReturnTokens()
  {
    // Arrange
    var user = new User("validUser", "user@example.com", "OldPassword@123", "John", "Doe", true);
    user.GenerateResetToken(); // Simulating token generation
    var command = new ChangeForgetPasswordCommand (user.ResetPasswordToken!, "NewPassword@123" );

    // Mock repository to return the user for the valid token
    _repository.SingleOrDefaultAsync(Arg.Any<UserByResetTokenSpec>(), Arg.Any<CancellationToken>())
        .Returns(user);

    // Mock successful login result
    var tokens = new Tokens { AccessToken = "access_token", RefreshToken = "refresh_token" };
    _loginService.LoginAsync(user, command.NewPassword).Returns(tokens);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsSuccess.Should().BeTrue();
    result.Value.Should().Be(tokens);

    // Verify that the user's password was recovered
    await _repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

    // Verify that the login service was called with the correct user and new password
    await _loginService.Received(1).LoginAsync(user, command.NewPassword);
  }

  // Test 3: Ensure that when a user is not found, no login attempt is made
  [Fact]
  public async Task Handle_WithInvalidToken_ShouldNotAttemptLogin()
  {
    // Arrange
    var command = new ChangeForgetPasswordCommand("invalid-token", "NewPassword123");
    _repository.SingleOrDefaultAsync(Arg.Any<UserByResetTokenSpec>(), Arg.Any<CancellationToken>())
        .Returns((User?)null);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Status.Should().Be(ResultStatus.NotFound);

    // Verify that no login attempt is made
    await _loginService.DidNotReceive().LoginAsync(Arg.Any<User>(), Arg.Any<string>());
  }
}
