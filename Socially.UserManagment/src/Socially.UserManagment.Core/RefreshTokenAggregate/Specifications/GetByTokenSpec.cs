using Ardalis.Specification;

namespace Socially.UserManagment.Core.RefreshTokenAggregate.Specifications;

public class GetByTokenSpec : Specification<RefreshToken>
{
  public GetByTokenSpec(string token)
  {
    Query.Where(t => t.Token == token);
  }
}
