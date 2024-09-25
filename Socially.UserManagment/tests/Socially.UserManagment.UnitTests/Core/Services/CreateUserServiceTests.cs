using Ardalis.SharedKernel;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Socially.UserManagment.Core.Services;
using Socially.UserManagment.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Events;
using Xunit;

namespace Socially.UserManagment.UnitTests.Core.Services;

public class CreateUserServiceTests
{
  private readonly IRepository<User> _repository;
  private readonly IMediator _mediator;
  private readonly ILogger<CreateUserService> _logger;
  private readonly CreateUserService _createUserService;

  public CreateUserServiceTests()
  {
    _repository = Substitute.For<IRepository<User>>();
    _mediator = Substitute.For<IMediator>();
    _logger = Substitute.For<ILogger<CreateUserService>>();
    _createUserService = new CreateUserService(
        _repository,
        _mediator,
        _logger);
  }

  // Test 1: Ensure that a valid user is created and a domain event is published
  [Fact]
  public async Task CreateUser_WithValidUser_ShouldReturnSuccessAndPublishEvent()
  {
    // Arrange
    var user = new User("validUser", "user@example.com", "Password@123", "John", "Doe", true);
    _repository.AddAsync(user).Returns(user);

    // Act
    var result = await _createUserService.CreateUser(user);

    // Assert
    result.IsSuccess.Should().BeTrue();
    result.Value.Should().Be(user.Id);

    // Verify that the user is added to the repository
    await _repository.Received(1).AddAsync(user);

    // Verify that the domain event is published
    await _mediator.Received(1).Publish(Arg.Is<UserCreatedEvent>(e => e.User == user), default);

    // Verify that the logger logs the correct message
    _logger.Received(1).Log(
        LogLevel.Information,
        Arg.Any<EventId>(),
        Arg.Is<object>(v => v.ToString()!.Contains("Creating A new User with an email of")),
        null,
        Arg.Any<Func<object, Exception?, string>>());
  }

  // Test 2: Ensure that the mediator is called to publish the event
  [Fact]
  public async Task CreateUser_ShouldPublishDomainEvent()
  {
    // Arrange
    var user = new User("validUser", "user@example.com", "Password@123", "John", "Doe", true);
    _repository.AddAsync(user).Returns(user);

    // Act
    await _createUserService.CreateUser(user);

    // Assert
    await _mediator.Received(1).Publish(Arg.Is<UserCreatedEvent>(e => e.User == user), default);
  }

  // Test 3: Ensure that the logger logs the correct message when creating a user
  [Fact]
  public async Task CreateUser_ShouldLogInformation()
  {
    // Arrange
    var user = new User("validUser", "user@example.com", "Password@123", "John", "Doe", true);
    _repository.AddAsync(user).Returns(user);

    // Act
    await _createUserService.CreateUser(user);

    // Assert
    _logger.Received(1).Log(
        LogLevel.Information,
        Arg.Any<EventId>(),
        Arg.Is<object>(v => v.ToString()!.Contains("Creating A new User with an email of")),
        null,
        Arg.Any<Func<object, Exception?, string>>());
  }
}
