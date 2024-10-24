using Ardalis.Specification;

namespace Socially.ContentManagment.Core.UserAggregate.Specifications;

public class UserByVerificationTokenSpec : Specification<User>, ISingleResultSpecification<User>
{
  public UserByVerificationTokenSpec(string token)
  {
    Query
      .Where(u => u.VerificationToken == token);
  }
}
