using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Socially.UserManagement.Core.UserAggregate;

namespace Socially.UserManagment.Core.UserAggregate.Specifications;
public class UserByResetTokenSpec : Specification<User>, ISingleResultSpecification<User>
{
  public UserByResetTokenSpec(string resetPasswordToken)
  {
    Query.Where(u => u.ResetPasswordToken == resetPasswordToken);
  }
}
