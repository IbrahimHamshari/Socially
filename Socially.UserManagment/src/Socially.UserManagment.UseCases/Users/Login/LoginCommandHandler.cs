using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Errors;
using Socially.UserManagment.Core.UserAggregate.Specifications;
using Socially.UserManagment.UseCases.Users.Common;

namespace Socially.UserManagment.UseCases.Users.Login;
public class LoginCommandHandler(IRepository<User> _repository, ILoginService _loginService) : ICommandHandler<LoginCommand, Result<Tokens>>
{
  public async Task<Result<Tokens>> Handle(LoginCommand request, CancellationToken cancellationToken)
  {
    var username = request.User.UserName;
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
