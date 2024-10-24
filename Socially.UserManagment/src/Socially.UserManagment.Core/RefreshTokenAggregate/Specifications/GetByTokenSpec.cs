using Ardalis.Specification;

namespace Socially.ContentManagment.Core.RefreshTokenAggregate.Specifications;

public class GetByTokenSpec : Specification<RefreshToken>
{
  public GetByTokenSpec(string token)
  {
    Query.Where(t => t.Token == token);
  }
}
