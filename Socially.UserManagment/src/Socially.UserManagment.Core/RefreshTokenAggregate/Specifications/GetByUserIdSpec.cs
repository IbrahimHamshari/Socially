using Ardalis.Specification;

namespace Socially.UserManagment.Core.RefreshTokenAggregate.Specifications;

public class GetByUserIdSpec : Specification<RefreshToken>
{
  public GetByUserIdSpec(Guid userId)
  {
    Query.Where(rt => rt.UserId == userId);
  }
}
