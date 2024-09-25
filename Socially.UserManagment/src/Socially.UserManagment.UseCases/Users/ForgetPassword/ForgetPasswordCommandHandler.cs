using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Ardalis.Specification;
using Microsoft.Extensions.Logging;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Errors;
using Socially.UserManagment.Core.UserAggregate.Specifications;

namespace Socially.UserManagment.UseCases.Users.ForgetPassword;
public class ForgetPasswordCommandHandler(
  IRepository<User> _repository) : ICommandHandler<ForgetPasswordCommand, Result>
{
  public async Task<Result> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
  {
    var email = request.Email;
    var spec = new UserByEmailSpec(email);
    var user = await _repository.SingleOrDefaultAsync(spec, cancellationToken);
    if (user == null)
    {
      return UserErrors.NotFoundByEmail(email);
    }
    user.GenerateResetToken();
    await _repository.SaveChangesAsync(cancellationToken);
      return Result.Success();

    }
}
