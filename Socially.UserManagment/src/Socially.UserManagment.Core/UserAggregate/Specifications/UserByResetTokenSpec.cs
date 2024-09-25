using Ardalis.Specification;

namespace Socially.UserManagment.Core.UserAggregate.Specifications;

public class UserByResetTokenSpec : Specification<User>, ISingleResultSpecification<User>
{
  public UserByResetTokenSpec(string resetPasswordToken)
  {
    Query.Where(u => u.ResetPasswordToken == resetPasswordToken);
  }
}
