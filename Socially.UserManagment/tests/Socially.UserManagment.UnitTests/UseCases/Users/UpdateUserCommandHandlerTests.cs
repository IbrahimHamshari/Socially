using Ardalis.Result;
using Ardalis.SharedKernel;
using FluentAssertions;
using NSubstitute;
using Socially.ContentManagment.Core.UserAggregate;
using Socially.ContentManagment.UseCases.Users.Common.DTOs;
using Socially.ContentManagment.UseCases.Users.Update;
using Xunit;

namespace Socially.ContentManagment.UnitTests.UseCases.Users;

public class UpdateUserCommandHandlerTests
{
  private readonly IRepository<User> _repository;
  private readonly UpdateUserCommandHandler _handler;

  public UpdateUserCommandHandlerTests()
  {
    _repository = Substitute.For<IRepository<User>>();
    _handler = new UpdateUserCommandHandler(_repository);
  }

  // Test 1: Ensure that user properties are updated when the user is found
  [Fact]
  public async Task Handle_WithValidUserId_ShouldUpdateUserAndReturnUpdatedDto()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var command = new UpdateUserCommand
      (
       userId,
      new UserUpdateDto
      {
        Email = "newemail@example.com",
        FirstName = "NewFirstName",
        LastName = "NewLastName",
        Bio = "New Bio",
        ProfilePictureURL = "newProfilePicUrl",
        CoverPhotoURL = "newCoverPhotoUrl",
        DateOfBirth = DateTime.UtcNow.AddYears(-30),
        Gender = true
      }
      )
    ;

    var user = Substitute.For<User>(userId, "validUser", "user@example.com", "Password@123", "John", "Doe", true);
    _repository.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<User?>(user));

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsSuccess.Should().BeTrue();
    result.Value.Should().NotBeNull();
    result.Value.Email.Should().Be(command.User.Email);
    result.Value.FirstName.Should().Be(command.User.FirstName);
    result.Value.LastName.Should().Be(command.User.LastName);
    result.Value.Bio.Should().Be(command.User.Bio);
    result.Value.ProfilePictureURL.Should().Be(command.User.ProfilePictureURL);
    result.Value.CoverPhotoURL.Should().Be(command.User.CoverPhotoURL);
    result.Value.DateOfBirth.Should().Be(command.User.DateOfBirth);
    result.Value.Gender.Should().Be(command.User.Gender);

    // Verify that the user was updated with the correct properties
    user.Received(1).UpdateEmail(command.User.Email!);
    user.Received(1).UpdateFirstName(command.User.FirstName!);
    user.Received(1).UpdateLastName(command.User.LastName!);
    user.Received(1).UpdateBio(command.User.Bio!);
    user.Received(1).UpdateProfilePictureURL(command.User.ProfilePictureURL!);
    user.Received(1).UpdateCoverPhotoURL(command.User.CoverPhotoURL!);
    user.Received(1).UpdateDateOfBirth(command.User.DateOfBirth!.Value);
    user.Received(1).UpdateGender(command.User.Gender!.Value);

    // Verify that SaveChangesAsync was called
    await _repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
  }

  // Test 2: Ensure that NotFound error is returned when user is not found
  [Fact]
  public async Task Handle_WithInvalidUserId_ShouldReturnNotFoundError()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var command = new UpdateUserCommand
    (userId,
      new UserUpdateDto { Email = "newemail@example.com" }
    );

    _repository.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<User?>(null));

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Status.Should().Be(ResultStatus.NotFound);
    result.Errors.Should().ContainSingle(e => e == $"The user with the Id = '{userId}' was not found");

    // Verify that SaveChangesAsync was never called since the user was not found
    await _repository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
  }
}
