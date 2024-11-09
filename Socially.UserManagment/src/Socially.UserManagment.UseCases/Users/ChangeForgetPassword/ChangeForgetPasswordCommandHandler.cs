using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.UserManagment.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Errors;
using Socially.UserManagment.Core.UserAggregate.Specifications;
using Socially.UserManagment.UseCases.Users.ChangePasswordForget;
using Socially.UserManagment.UseCases.Users.Common;
using Socially.UserManagment.UseCases.Users.Login;

namespace Socially.UserManagment.UseCases.Users.ChangeForgetPassword;

public class ChangeForgetPasswordCommandHandler(IRepository<User> _repository,
  ILoginService _loginService) : ICommandHandler<ChangeForgetPasswordCommand, Result<Tokens>>
{
  public async Task<Result<Tokens>> Handle(ChangeForgetPasswordCommand request, CancellationToken cancellationToken)
  {
    var token = request.Token;
    var password = request.NewPassword;
    var spec = new UserByResetTokenSpec(token);
    var user = await _repository.SingleOrDefaultAsync(spec, cancellationToken);
    if (user == null)
    {
      return UserErrors.NotFoundByResetToken(token);
    }
    user.RecoverAccount(token, password);
    await _repository.SaveChangesAsync(cancellationToken);

    var loginResult = await _loginService.LoginAsync(user, request.NewPassword);
    return Result.Success(loginResult);
  }
}
