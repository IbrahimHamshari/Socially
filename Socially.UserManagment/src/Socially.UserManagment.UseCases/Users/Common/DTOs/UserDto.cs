namespace Socially.UserManagment.UseCases.Users.Common.DTOs;

public class UserDto
{
  public required Guid Id { get; init; }
  public required string Email { get; init; }
  public required string FirstName { get; init; }
  public required string LastName { get; init; }
  public required string Bio { get; init; }
  public string? ProfilePictureURL { get; init; }
  public string? CoverPhotoURL { get; init; }
  public DateTimeOffset? DateOfBirth { get; init; }
  public required bool Gender { get; init; }
}
