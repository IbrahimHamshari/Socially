using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.UserAggregate;
using Socially.ContentManagment.Core.UserAggregate.Errors;
using Socially.ContentManagment.Core.UserAggregate.Specifications;

namespace Socially.ContentManagment.UseCases.Users.ForgetPassword;

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
