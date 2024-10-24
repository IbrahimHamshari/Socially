using Ardalis.Result;
using Ardalis.SharedKernel;
using FluentAssertions;
using NSubstitute;
using Socially.ContentManagment.Core.UserAggregate;
using Socially.ContentManagment.Core.UserAggregate.Specifications;
using Socially.ContentManagment.UseCases.Users.ForgetPassword;
using Xunit;

namespace Socially.ContentManagment.UnitTests.UseCases.Users;

public class ForgetPasswordCommandHandlerTests
{
  private readonly IRepository<User> _repository;
  private readonly ForgetPasswordCommandHandler _handler;

  public ForgetPasswordCommandHandlerTests()
  {
    _repository = Substitute.For<IRepository<User>>();
    _handler = new ForgetPasswordCommandHandler(_repository);
  }

  // Test 1: Ensure that if the user is found by email, a reset token is generated and saved
  [Fact]
  public async Task Handle_WithValidEmail_ShouldGenerateResetTokenAndSave()
  {
    // Arrange
    var user = new User(Guid.NewGuid(), "validUser", "user@example.com", "Password@123", "John", "Doe", true);
    var command = new ForgetPasswordCommand("user@example.com");

    // Mock repository to return the user for the valid email
    _repository.SingleOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>())
        .Returns(user);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsSuccess.Should().BeTrue();

    // Verify that the user's reset token was generated
    user.ResetPasswordToken.Should().NotBeNullOrEmpty();

    // Verify that the repository save method was called
    await _repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
  }

  // Test 2: Ensure that if the user is not found, the proper error is returned
  [Fact]
  public async Task Handle_WithInvalidEmail_ShouldReturnNotFoundError()
  {
    // Arrange
    var command = new ForgetPasswordCommand("nonexistent@example.com");

    // Mock repository to return null for the non-existent email
    _repository.SingleOrDefaultAsync(Arg.Any<UserByEmailSpec>(), Arg.Any<CancellationToken>())
        .Returns((User?)null);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Status.Should().Be(ResultStatus.NotFound);
    result.Errors.Should().ContainSingle(e => e == $"The user with Email = '{command.Email}' was not found");
  }
}
