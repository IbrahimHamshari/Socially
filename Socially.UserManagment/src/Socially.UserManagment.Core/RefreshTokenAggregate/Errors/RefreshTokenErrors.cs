using Ardalis.Result;

namespace Socially.UserManagment.Core.RefreshTokenAggregate.Errors;

public static class RefreshTokenErrors
{
  public static Result NotFoundByUserId(string userId) => Result.NotFound("RefreshTokens.NotFoundByUserId", $"The Token with the User Id = '{userId}' was not found");

  public static Result NotFoundByToken(string token) => Result.NotFound("RefreshTokens.NotFoundByToken", $"The Token with the token = '{token}' was not found");

  public static Result NotFoundByFamily(string family) => Result.NotFound("RefreshTokens.NotFoundByFamily", $"The Token with the family = '{family}' was not found");
}
