using Ardalis.Specification;

namespace Socially.ContentManagment.Core.RefreshTokenAggregate.Specifications;

public class GetByFamilySpec : Specification<RefreshToken>
{
  public GetByFamilySpec(string family)
  {
    Query.Where(t => t.Family == family);
  }
}
