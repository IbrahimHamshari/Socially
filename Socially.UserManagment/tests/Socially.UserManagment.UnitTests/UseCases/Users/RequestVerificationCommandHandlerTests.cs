using Ardalis.Result;
using Ardalis.SharedKernel;
using FluentAssertions;
using NSubstitute;
using Socially.UserManagment.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Specifications;
using Socially.UserManagment.UseCases.Users.RequestVerification;
using Xunit;

namespace Socially.UserManagment.UnitTests.UseCases.Users;

public class RequestVerificationCommandHandlerTests
{
  private readonly IRepository<User> _repository;
  private readonly RequestVerificationCommandHandler _handler;

  public RequestVerificationCommandHandlerTests()
  {
    _repository = Substitute.For<IRepository<User>>();
    _handler = new RequestVerificationCommandHandler(_repository);
  }

  // Test 1: Ensure the verification token is generated when the user is found
  [Fact]
  public async Task Handle_WithValidUserId_ShouldGenerateVerificationToken()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var command = new RequestVerificationCommand(userId);

    var user = new User("validUser", "user@example.com", "Password@123", "John", "Doe", true);
    _repository.SingleOrDefaultAsync(Arg.Any<UserByIdSpec>()).Returns(Task.FromResult<User?>(user));

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsSuccess.Should().BeTrue();
    user.VerificationToken.Should().NotBeNullOrEmpty();
    user.TokenGeneratedAt.Should().NotBeNull();

    // Verify the repository was called with the correct specification
    await _repository.Received(1).SingleOrDefaultAsync(Arg.Is<UserByIdSpec>(spec => spec.GetType() == typeof(UserByIdSpec)));
  }

  // Test 2: Ensure an error is returned when the user is not found
  [Fact]
  public async Task Handle_WithInvalidUserId_ShouldReturnNotFoundError()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var command = new RequestVerificationCommand(userId);

    _repository.SingleOrDefaultAsync(Arg.Any<UserByIdSpec>()).Returns(Task.FromResult<User?>(null));

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Status.Should().Be(ResultStatus.NotFound);
    result.Errors.Should().ContainSingle(e => e == $"The user with the Id = '{userId}' was not found");

    // Verify the repository was called with the correct specification
    await _repository.Received(1).SingleOrDefaultAsync(Arg.Is<UserByIdSpec>(spec => spec.GetType() == typeof(UserByIdSpec)));
  }
}
