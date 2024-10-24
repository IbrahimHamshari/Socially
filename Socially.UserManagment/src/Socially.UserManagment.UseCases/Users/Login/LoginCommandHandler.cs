﻿using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.UserAggregate;
using Socially.ContentManagment.Core.UserAggregate.Errors;
using Socially.ContentManagment.Core.UserAggregate.Specifications;
using Socially.ContentManagment.UseCases.Users.Common;

namespace Socially.ContentManagment.UseCases.Users.Login;

public class LoginCommandHandler(IRepository<User> _repository, ILoginService _loginService) : ICommandHandler<LoginCommand, Result<Tokens>>
{
  public async Task<Result<Tokens>> Handle(LoginCommand request, CancellationToken cancellationToken)
  {
    var username = request.User.Username;
    var password = request.User.Password;
    var spec = new UserByUsernameSpec(username);
    var user = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (user == null)
    {
      return UserErrors.NotFoundByUsername(username);
    }

    var loginResult = await _loginService.LoginAsync(user, password);
    return loginResult;
  }
}
