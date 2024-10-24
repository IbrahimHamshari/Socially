using Ardalis.Specification;

namespace Socially.ContentManagment.Core.UserAggregate.Specifications;

public class UserByUsernameSpec : Specification<User>, ISingleResultSpecification<User>
{
  public UserByUsernameSpec(string username)
  {
    Query.Where(user => user.Username == username);
  }
}
