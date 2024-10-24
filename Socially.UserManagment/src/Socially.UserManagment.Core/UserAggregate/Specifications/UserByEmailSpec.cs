using Ardalis.Specification;

namespace Socially.ContentManagment.Core.UserAggregate.Specifications;

public class UserByEmailSpec : Specification<User>, ISingleResultSpecification<User>
{
  public UserByEmailSpec(string email)
  {
    Query.Where(u => u.Email == email);
  }
}
