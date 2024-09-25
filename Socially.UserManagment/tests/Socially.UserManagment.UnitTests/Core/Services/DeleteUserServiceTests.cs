using Xunit;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR;
using Ardalis.Result;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.Core.Interfaces;
using Socially.UserManagment.Core.Services;
using Socially.UserManagment.Core.UserAggregate.Events;
using Ardalis.SharedKernel;
using FluentAssertions;

namespace Socially.UserManagment.UnitTests.Core.Services;

public class DeleteUserServiceTests
{
  private readonly IRepository<User> _repository;
  private readonly IMediator _mediator;
  private readonly ILogger<DeleteUserService> _logger;
  private readonly DeleteUserService _deleteUserService;

  public DeleteUserServiceTests()
  {
    _repository = Substitute.For<IRepository<User>>();
    _mediator = Substitute.For<IMediator>();
    _logger = Substitute.For<ILogger<DeleteUserService>>();
    _deleteUserService = new DeleteUserService(_repository, _mediator, _logger);
  }
  [Fact]
  public async Task DeleteUser_WithValidUserId_ShouldDeleteUserAndPublishEvent()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var user = new User("validUser", "user@example.com", "Password@123", "John", "Doe", true);
    _repository.GetByIdAsync(userId).Returns(user);

    // Act
    var result = await _deleteUserService.DeleteUser(userId);

    // Assert
    result.IsSuccess.Should().BeTrue();

    // Verify that the user is deleted from the repository
    await _repository.Received(1).DeleteAsync(user);

    // Verify that the domain event is published
    await _mediator.Received(1).Publish(Arg.Is<UserDeletedEvent>(e => e.User == user), default);

    // Verify that the logger logs the correct message
    _logger.Received(1).Log(
        LogLevel.Information,
        Arg.Any<EventId>(),
        Arg.Is<object>(v => v.ToString()!.Contains($"Deleteing User {userId}")),
        null,
        Arg.Any<Func<object, Exception?, string>>());
  }

  // Test 2: Ensure that trying to delete a non-existent user returns NotFound
  [Fact]
  public async Task DeleteUser_WithNonExistentUserId_ShouldReturnNotFound()
  {
    // Arrange
    var userId = Guid.NewGuid();
    _repository.GetByIdAsync(userId).Returns((User?)null);

    // Act
    var result = await _deleteUserService.DeleteUser(userId);

    // Assert
    result.Status.Should().Be(ResultStatus.NotFound);

    // Verify that the repository's DeleteAsync is never called
    await _repository.DidNotReceive().DeleteAsync(Arg.Any<User>());

    // Verify that the domain event is never published
    await _mediator.DidNotReceive().Publish(Arg.Any<UserDeletedEvent>(), default);

    // Verify that the logger logs the correct message
    _logger.Received(1).Log(
        LogLevel.Information,
        Arg.Any<EventId>(),
        Arg.Is<object>(v => v.ToString()!.Contains($"Deleteing User {userId}")),
        null,
        Arg.Any<Func<object, Exception?, string>>());
  }


  // Test 3: Ensure that the logger logs the correct information
  [Fact]
  public async Task DeleteUser_ShouldLogInformation()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var user = new User("validUser", "user@example.com", "Password@123", "John", "Doe", true);
    _repository.GetByIdAsync(userId).Returns(user);

    // Act
    await _deleteUserService.DeleteUser(userId);

    // Assert
    _logger.Received(1).Log(
        LogLevel.Information,
        Arg.Any<EventId>(),
        Arg.Is<object>(v => v.ToString()!.Contains($"Deleteing User {userId}")),
        null,
        Arg.Any<Func<object, Exception?, string>>());
  }
}
