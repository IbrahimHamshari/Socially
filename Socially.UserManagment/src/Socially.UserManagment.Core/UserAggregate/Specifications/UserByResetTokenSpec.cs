using Ardalis.Specification;

namespace Socially.ContentManagment.Core.UserAggregate.Specifications;

public class UserByResetTokenSpec : Specification<User>, ISingleResultSpecification<User>
{
  public UserByResetTokenSpec(string resetPasswordToken)
  {
    Query.Where(u => u.ResetPasswordToken == resetPasswordToken);
  }
}
