using Ardalis.Result;

namespace Socially.UserManagment.Core.UserAggregate.Errors;

public static class UserErrors
{
  public static Result NotFound(Guid userId) => Result.NotFound("Users.NotFound", $"The user with the Id = '{userId}' was not found");

  public static Result NotFoundByEmail(string email) => Result.NotFound("Users.NotFoundByEmail", $"The user with Email = '{email}' was not found");

  public static Result NotFoundByResetToken(string resetToken) => Result.NotFound("Users.NotFoundByResetToken", $"The user with Reset Token = '{resetToken}' was not found");

  public static Result NotFoundByUsername(string username) => Result.NotFound("Users.NotFoundByUsername", $"The user with Username = '{username}' was not found");

  public static Result NotFoundByVerificationToken(string verificationToken) => Result.NotFound("Users.NotFoundByVerificationToken", $"The user with Verification Token = '{verificationToken}' was not found");
}
