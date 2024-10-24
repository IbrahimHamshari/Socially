using Socially.ContentManagment.Core.RefreshTokenAggregate;

namespace Socially.ContentManagment.UseCases.Users.Interfaces;

public interface ITokenGenerator
{
  string GenerateAccessToken(Guid userId);

  Task<RefreshToken> GenerateRefreshToken(Guid userId, string? parentToken = null);
}
