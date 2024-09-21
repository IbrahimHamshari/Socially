using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Specifications;

namespace Socially.UserManagment.UseCases.Users.Verify;
public class VerifyCommandHandler(IRepository<User> _repository) : ICommandHandler<VerifyCommand, Result>
{
  public async Task<Result> Handle(VerifyCommand request, CancellationToken cancellationToken)
  {
    var spec = new UserByVerificationTokenSpec(request.token);
    var user = await _repository.SingleOrDefaultAsync(spec, cancellationToken);
    if (user == null)
    {
      return Result.Invalid(new ValidationError("No Such User Exist!"));
    }
    user.VerifyEmail(request.token);
    await _repository.SaveChangesAsync(cancellationToken);
    return Result.Success();
  }
}
