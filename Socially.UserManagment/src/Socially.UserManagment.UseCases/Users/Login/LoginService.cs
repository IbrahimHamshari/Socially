using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Specifications;
using Socially.UserManagment.Shared.Config.JWT;
using Socially.UserManagment.UseCases.Users.Common;
using Socially.UserManagment.UseCases.Users.Interfaces;

namespace Socially.UserManagment.UseCases.Users.Login;
public class LoginService( 
  ITokenGenerator TokenGenerator,
  ILogger<LoginService> Logger
  ) : ILoginService
{

  public async Task<Result<Tokens>> LoginAsync(User user, string password)
  {
    bool isEqual = user.VerifyPassword(password);

    if(!isEqual)
    {
      return Result.Unauthorized();
    }
    Logger.LogInformation("User with ID of {id} has logged In", user.Id);
    if (!user.IsActive)
    {
      return Result.Unauthorized();
    }
    user.RecordLogin();
    var refershToken = await TokenGenerator.GenerateRefreshToken(user.Id);
    var accessToken = TokenGenerator.GenerateAccessToken(user.Id);
    return Result.Success(new Tokens { AccessToken= accessToken, RefreshToken= refershToken.Token });
  }
}
