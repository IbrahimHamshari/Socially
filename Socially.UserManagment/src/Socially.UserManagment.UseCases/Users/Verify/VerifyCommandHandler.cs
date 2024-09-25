using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.UserManagment.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Errors;
using Socially.UserManagment.Core.UserAggregate.Specifications;

namespace Socially.UserManagment.UseCases.Users.Verify;

public class VerifyCommandHandler(IRepository<User> _repository) : ICommandHandler<VerifyCommand, Result>
{
  public async Task<Result> Handle(VerifyCommand request, CancellationToken cancellationToken)
  {
    var token = request.Token;
    var spec = new UserByVerificationTokenSpec(request.Token);
    var user = await _repository.SingleOrDefaultAsync(spec, cancellationToken);
    if (user == null)
    {
      return UserErrors.NotFoundByVerificationToken(request.Token);
    }
    user.VerifyEmail(request.Token);
    await _repository.SaveChangesAsync(cancellationToken);
    return Result.Success();
  }
}
