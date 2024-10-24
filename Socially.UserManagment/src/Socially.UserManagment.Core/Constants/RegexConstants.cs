namespace Socially.ContentManagment.Core.Constants;

public class RegexConstants
{
  // NOTE: Use an environment variable in a production app
  public const string EMAIL_REGEX = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

  public const string PASSWORD_REGEX = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,24}$";
  public const string USERNAME_REGEX = "^[a-zA-Z0-9]{6,18}$";
  public const string NAME_REGEX = "^[a-zA-Z\u0600-\u06FF]{2,16}$";
}
