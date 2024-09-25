using Socially.UserManagment.Core.Constants;

namespace Ardalis.GuardClauses;

public static class UserGuardExtension
{
  public static string InvalidEmailFormat(this IGuardClause guardClause, string email, string parameterName)
  {
    return Guard.Against.InvalidFormat(email, parameterName, RegexConstants.EMAIL_REGEX);
  }

  public static string InvalidPasswordFormat(this IGuardClause guardClause, string password, string parameterName)
  {
    return Guard.Against.InvalidFormat(password, parameterName, RegexConstants.PASSWORD_REGEX);
  }

  public static string InvalidUserNameFormat(this IGuardClause guardClause, string userName, string parameterName)
  {
    return Guard.Against.InvalidFormat(userName, parameterName, RegexConstants.USERNAME_REGEX);
  }

  public static string InvalidNameFormat(this IGuardClause guardClause, string name, string parameterName)
  {
    return Guard.Against.InvalidFormat(name, parameterName, RegexConstants.NAME_REGEX);
  }
}
