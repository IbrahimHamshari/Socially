using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Socially.UserManagement.Core.UserAggregate;

namespace Socially.UserManagment.Core.UserAggregate.Specifications;
public class GetByUsernameSpec : Specification<User>
{
  public GetByUsernameSpec(string username)
  {
    Query.Where(user => user.Username == username);
  }
}
