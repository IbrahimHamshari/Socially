using Ardalis.Result;
using Ardalis.SharedKernel;
using FluentAssertions;
using NSubstitute;
using Socially.UserManagment.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Specifications;
using Socially.UserManagment.UseCases.Users.Get;
using Xunit;

namespace Socially.UserManagment.UnitTests.UseCases.Users;

public class GetUserQueryHandlerTests
{
  private readonly IReadRepository<User> _repository;
  private readonly GetUserQueryHandler _handler;

  public GetUserQueryHandlerTests()
  {
    _repository = Substitute.For<IReadRepository<User>>();
    _handler = new GetUserQueryHandler(_repository);
  }

  // Test 1: Ensure that if the user is found by ID, the correct UserDto is returned
  [Fact]
  public async Task Handle_WithValidUserId_ShouldReturnUserDto()
  {
    // Arrange
    var user = new User(Guid.NewGuid(), "validUser", "user@example.com", "Password@123", "John", "Doe", true);
    user.UpdateBio("This is a bio");
    user.UpdateCoverPhotoURL("cover-photo-url");
    user.UpdateDateOfBirth(DateTimeOffset.UtcNow.AddYears(-30));
    user.UpdateProfilePictureURL("profile-picture-url");
    var command = new GetUserQuery(user.Id);

    // Mock repository to return the user for the valid ID
    _repository.FirstOrDefaultAsync(Arg.Any<UserByIdSpec>(), Arg.Any<CancellationToken>())
        .Returns(user);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsSuccess.Should().BeTrue();
    var userDto = result.Value;

    // Verify that the correct UserDto is returned
    userDto.Should().NotBeNull();
    userDto.Id.Should().Be(user.Id);
    userDto.Bio.Should().Be(user.Bio);
    userDto.Email.Should().Be(user.Email);
    userDto.FirstName.Should().Be(user.FirstName);
    userDto.LastName.Should().Be(user.LastName);
    userDto.Gender.Should().Be(user.Gender);
    userDto.CoverPhotoURL.Should().Be(user.CoverPhotoURL);
    userDto.DateOfBirth.Should().Be(user.DateOfBirth);
    userDto.ProfilePictureURL.Should().Be(user.ProfilePictureURL);
  }

  // Test 2: Ensure that if the user is not found, the proper error is returned
  [Fact]
  public async Task Handle_WithInvalidUserId_ShouldReturnNotFoundError()
  {
    // Arrange
    var command = new GetUserQuery(Guid.NewGuid());

    // Mock repository to return null for the non-existent user
    _repository.FirstOrDefaultAsync(Arg.Any<UserByIdSpec>(), Arg.Any<CancellationToken>())
        .Returns((User?)null);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Status.Should().Be(ResultStatus.NotFound);
    result.Errors.Should().ContainSingle(e => e == $"The user with the Id = '{command.Id}' was not found");
  }
}
