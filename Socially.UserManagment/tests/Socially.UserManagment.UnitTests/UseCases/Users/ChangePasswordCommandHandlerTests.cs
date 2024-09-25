using Ardalis.Result;
using Ardalis.SharedKernel;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Socially.UserManagment.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Specifications;
using Socially.UserManagment.UseCases.Users.ChangePassword;
using Socially.UserManagment.UseCases.Users.Common.DTOs;
using Xunit;

namespace Socially.UserManagment.UnitTests.UseCases.Users;

public class ChangePasswordCommandHandlerTests
{
  private readonly IRepository<User> _repository;
  private readonly ILogger<ChangePasswordCommandHandler> _logger;
  private readonly ChangePasswordCommandHandler _handler;

  public ChangePasswordCommandHandlerTests()
  {
    _repository = Substitute.For<IRepository<User>>();
    _logger = Substitute.For<ILogger<ChangePasswordCommandHandler>>();
    _handler = new ChangePasswordCommandHandler(_repository, _logger);
  }

  // Test 1: Ensure the user is found and their password is successfully changed
  [Fact]
  public async Task Handle_WithValidUserId_ShouldChangePasswordAndSave()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var user = new User("validUser", "user@example.com", "OldPassword@123", "John", "Doe", true);
    var command = new ChangePasswordCommand
    (
       userId,
       new ChangePasswordDto
       {
         CurrentPassword = "OldPassword@123",
         Password = "NewPassword@123"
       }
    );

    _repository.SingleOrDefaultAsync(Arg.Any<UserByIdSpec>(), Arg.Any<CancellationToken>()).Returns(user);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsSuccess.Should().BeTrue();

    // Verify the password change and repository save
    await _repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

    // Verify that the logger logs the correct message
    _logger.Received(1).Log(
        LogLevel.Information,
        Arg.Any<EventId>(),
        Arg.Is<object>(v => v.ToString()!.Contains($"Password Has Changed for the user with Id of {user.Id}")),
        null,
        Arg.Any<Func<object, Exception?, string>>());
  }

  // Test 2: Ensure that if the user is not found, the proper error is returned
  [Fact]
  public async Task Handle_WithInvalidUserId_ShouldReturnNotFoundError()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var command = new ChangePasswordCommand
    (
     userId,
     new ChangePasswordDto
     {
       CurrentPassword = "OldPassword@123",
       Password = "NewPassword@123"
     }
    );

    _repository.SingleOrDefaultAsync(Arg.Any<UserByIdSpec>(), Arg.Any<CancellationToken>())
        .Returns((User?)null);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Status.Should().Be(ResultStatus.NotFound);
    result.IsNotFound();
  }

  // Test 3: Ensure that if the user is found but the current password is wrong, an error is returned
  [Fact]
  public async Task Handle_WithWrongCurrentPassword_ShouldThrowUnauthorizedError()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var user = new User("validUser", "user@example.com", "OldPassword@123", "John", "Doe", true);
    var command = new ChangePasswordCommand
    (
      userId,
      new ChangePasswordDto
      {
        CurrentPassword = "WrongPassword@123",
        Password = "NewPassword@123"
      }
    );

    _repository.SingleOrDefaultAsync(Arg.Any<UserByIdSpec>(), Arg.Any<CancellationToken>()).Returns(user);

    // Act
    Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

    // Assert
    await act.Should().ThrowAsync<UnauthorizedAccessException>()
             .WithMessage("Current password is incorrect.");

    // Verify the password change was not saved
    await _repository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
  }
}
