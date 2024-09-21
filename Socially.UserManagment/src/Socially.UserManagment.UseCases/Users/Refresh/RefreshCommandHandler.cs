using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;
using Socially.UserManagment.Core.RefreshTokenAggregate;
using Socially.UserManagment.Core.RefreshTokenAggregate.Specifications;
using Socially.UserManagment.UseCases.Users.Interfaces;

namespace Socially.UserManagment.UseCases.Users.Refresh;
public class RefreshCommandHandler(IRepository<RefreshToken> Repository,
  ITokenGenerator tokenGenerator,
  ILogger<RefreshCommandHandler> Logger) : ICommandHandler<RefreshCommand, Result<string[]>>
{
  public async Task<Result<string[]>> Handle(RefreshCommand request, CancellationToken cancellationToken)
  {
    var token = request.refreshToken;
    var refreshToken = await Repository.FirstOrDefaultAsync(new GetByTokenSpec(token), cancellationToken);
    if (refreshToken == null || refreshToken.IsExpired)
    {
      return Result.Unauthorized();
    }

    if(refreshToken.IsRevoked)
    {

      Logger.LogCritical("Someone Tried to use an Old Refresh Token {refreshToken} for the user of {user}", refreshToken.Id, refreshToken.UserId);
      var spec = new GetByFamilySpec(refreshToken.Family);
      await Repository.UpdateRangeAsync(await Repository.ListAsync(spec, cancellationToken));
      return Result.Unauthorized();
    }

    var newRefreshToken = await tokenGenerator.GenerateRefreshToken(refreshToken.UserId, refreshToken.Token);
    var accessToken = tokenGenerator.GenerateAccessToken(refreshToken.UserId);
    return Result.Success(new string[] { accessToken, newRefreshToken.Token });
  }
}
