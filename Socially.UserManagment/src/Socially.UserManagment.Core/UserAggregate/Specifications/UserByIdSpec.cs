using Ardalis.Specification;

namespace Socially.UserManagment.Core.UserAggregate.Specifications;

public class UserByIdSpec : Specification<User>, ISingleResultSpecification<User>
{
  public UserByIdSpec(Guid id)
  {
    Query.Where(user => user.Id == id);
  }
}
