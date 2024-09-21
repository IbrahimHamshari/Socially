using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Ardalis.Specification;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Specifications;

namespace Socially.UserManagment.UseCases.Users.ForgetPassword;
public class ForgetPasswordCommandHnadler(
  IRepository<User> _repository) : ICommandHandler<ForgetPasswordCommand, Result>
{
  public async Task<Result> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
  {
    var spec = new UserByEmailSpec(request.Email);
    var user = await _repository.SingleOrDefaultAsync(spec, cancellationToken);
    if (user == null)
    {
      return Result.NotFound();
    }
    user.GenerateResetToken();
    await _repository.SaveChangesAsync(cancellationToken);
    return Result.Success();
  }
}
