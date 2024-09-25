using Xunit;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.UseCases.Users.Register;
using FluentAssertions;
using Ardalis.SharedKernel;
using Socially.UserManagment.UseCases.Users.Common.DTOs;

namespace Socially.UserManagment.UnitTests.UseCases.Users;

public class RegisterUserCommandHandlerTests
{
  private readonly IRepository<User> _repository;
  private readonly RegisterUserCommandHandler _handler;

  public RegisterUserCommandHandlerTests()
  {
    _repository = Substitute.For<IRepository<User>>();
    _handler = new RegisterUserCommandHandler(_repository);
  }

  // Test 1: Ensure a user is registered and added to the repository
  [Fact]
  public async Task Handle_WithValidRequest_ShouldAddUserAndReturnCreatedResult()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var request = new RegisterUserCommand(
        new UserRegistrationDto
        {
          Username = "validUser",
          Email = "user@example.com",
          Password = "Password@123",
          FirstName = "John",
          LastName = "Doe",
          Gender = true
        }
    );

    var newUser = new User(request.User.Username, request.User.Email, request.User.Password, request.User.FirstName, request.User.LastName, request.User.Gender);

    _repository.AddAsync(Arg.Any<User>()).Returns(Task.FromResult(newUser));

    // Act
    var result = await _handler.Handle(request, CancellationToken.None);

    // Assert
    result.IsSuccess.Should().BeTrue();
    result.Status.Should().Be(ResultStatus.Created);
    result.Value.Should().Be(newUser.Id);

    // Verify the user was added to the repository
    await _repository.Received(1).AddAsync(Arg.Is<User>(u =>
        u.Username == request.User.Username &&
        u.Email == request.User.Email &&
        u.FirstName == request.User.FirstName &&
        u.LastName == request.User.LastName &&
        u.Gender == request.User.Gender));
  }

  // Test 2: Ensure a verification email token is generated for the new user
  [Fact]
  public async Task Handle_WithValidRequest_ShouldGenerateEmailVerificationToken()
  {
    // Arrange
    var request = new RegisterUserCommand(
        new UserRegistrationDto
        {
          Username = "validUser",
          Email = "user@example.com",
          Password = "Password@123",
          FirstName = "John",
          LastName = "Doe",
          Gender = true
        }
    );

    User? capturedUser = null;
    _repository.AddAsync(Arg.Do<User>(x => capturedUser = x)).Returns(Task.FromResult(new User(
        request.User.Username, request.User.Email, request.User.Password, request.User.FirstName, request.User.LastName, request.User.Gender)));

    // Act
    var result = await _handler.Handle(request, CancellationToken.None);

    // Assert
    result.IsSuccess.Should().BeTrue();
    capturedUser.Should().NotBeNull();
    capturedUser!.VerificationToken.Should().NotBeNullOrEmpty();
    capturedUser.TokenGeneratedAt.Should().NotBeNull();
  }
}
