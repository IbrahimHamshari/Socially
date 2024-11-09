using Socially.UserManagment.Core.RefreshTokenAggregate;

namespace Socially.UserManagment.UseCases.Users.Interfaces;

public interface ITokenGenerator
{
  string GenerateAccessToken(Guid userId);

  Task<RefreshToken> GenerateRefreshToken(Guid userId, string? parentToken = null);
}
