using Ardalis.Result;
using Ardalis.SharedKernel;
using FluentAssertions;
using NSubstitute;
using Socially.UserManagment.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Specifications;
using Socially.UserManagment.UseCases.Users.Verify;
using Xunit;

namespace Socially.UserManagment.UnitTests.UseCases.Users;

public class VerifyCommandHandlerTests
{
  private readonly IRepository<User> _repository;
  private readonly VerifyCommandHandler _handler;

  public VerifyCommandHandlerTests()
  {
    _repository = Substitute.For<IRepository<User>>();
    _handler = new VerifyCommandHandler(_repository);
  }

  // Test 1: Ensure that email is verified when the user is found by verification token
  [Fact]
  public async Task Handle_WithValidVerificationToken_ShouldVerifyEmailAndReturnSuccess()
  {
    // Arrange
    var user = Substitute.For<User>("validUser", "user@example.com", "Password@123", "John", "Doe", true);
    user.GenerateEmailVerificationToken();
    var token = user.VerificationToken;
    var command = new VerifyCommand(token!);
    _repository.SingleOrDefaultAsync(Arg.Any<UserByVerificationTokenSpec>(), Arg.Any<CancellationToken>())
        .Returns(Task.FromResult<User?>(user));

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsSuccess.Should().BeTrue();

    // Verify that SaveChangesAsync was called
    await _repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
  }

  // Test 2: Ensure that NotFound error is returned when the user is not found by verification token
  [Fact]
  public async Task Handle_WithInvalidVerificationToken_ShouldReturnNotFoundError()
  {
    // Arrange
    var token = "invalid-token";
    var command = new VerifyCommand(token);

    _repository.SingleOrDefaultAsync(Arg.Any<UserByVerificationTokenSpec>(), Arg.Any<CancellationToken>())
        .Returns(Task.FromResult<User?>(null));

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Status.Should().Be(ResultStatus.NotFound);
    result.IsNotFound();

    // Verify that SaveChangesAsync was never called since the user was not found
    await _repository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
  }
}
