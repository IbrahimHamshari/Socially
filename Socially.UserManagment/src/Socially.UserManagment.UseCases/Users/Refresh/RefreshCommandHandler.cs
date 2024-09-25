using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;
using Socially.UserManagment.Core.RefreshTokenAggregate;
using Socially.UserManagment.Core.RefreshTokenAggregate.Specifications;
using Socially.UserManagment.UseCases.Users.Interfaces;

namespace Socially.UserManagment.UseCases.Users.Refresh;

public class RefreshCommandHandler(IRepository<RefreshToken> _repository,
  ITokenGenerator _tokenGenerator,
  ILogger<RefreshCommandHandler> _logger) : ICommandHandler<RefreshCommand, Result<string[]>>
{
  public async Task<Result<string[]>> Handle(RefreshCommand request, CancellationToken cancellationToken)
  {
    var token = request.refreshToken;
    var refreshToken = await _repository.FirstOrDefaultAsync(new GetByTokenSpec(token), cancellationToken);
    if (refreshToken == null || refreshToken.IsExpired)
    {
      return Result.Unauthorized();
    }

    if (refreshToken.IsRevoked)
    {
      _logger.LogCritical("Someone Tried to use an Old Refresh Token {refreshToken} for the user of {user}", refreshToken.Id, refreshToken.UserId);
      var spec = new GetByFamilySpec(refreshToken.Family);
      await _repository.UpdateRangeAsync(await _repository.ListAsync(spec, cancellationToken));
      return Result.Unauthorized();
    }

    var newRefreshToken = await _tokenGenerator.GenerateRefreshToken(refreshToken.UserId, refreshToken.Token);
    var accessToken = _tokenGenerator.GenerateAccessToken(refreshToken.UserId);
    return Result.Success(new string[] { accessToken, newRefreshToken.Token });
  }
}
