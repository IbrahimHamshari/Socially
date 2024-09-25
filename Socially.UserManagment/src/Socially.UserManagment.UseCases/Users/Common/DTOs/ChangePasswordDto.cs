namespace Socially.UserManagment.UseCases.Users.Common.DTOs;

public class ChangePasswordDto
{
  public required string CurrentPassword { get; init; }
  public required string Password { get; init; }
}
