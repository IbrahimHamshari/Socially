using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Specifications;

namespace Socially.UserManagment.UseCases.Users.Login;
public class LoginCommandHandler(IRepository<User> _repository, ILoginService _loginService) : ICommandHandler<LoginCommand, Result<string[]>>
{
  public async Task<Result<string[]>> Handle(LoginCommand request, CancellationToken cancellationToken)
  {
    var spec = new GetByUsernameSpec(request.Username);
    var user = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (user == null)
    {
      return Result.NotFound();
    }

    var loginResult = await _loginService.LoginAsync(user, request.Password);

    return Result.Success(loginResult);
  }
}
