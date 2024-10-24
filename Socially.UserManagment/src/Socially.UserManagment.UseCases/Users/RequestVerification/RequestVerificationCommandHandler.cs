using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.UserAggregate;
using Socially.ContentManagment.Core.UserAggregate.Errors;
using Socially.ContentManagment.Core.UserAggregate.Specifications;

namespace Socially.ContentManagment.UseCases.Users.RequestVerification;

public class RequestVerificationCommandHandler(IRepository<User> _repository) : ICommandHandler<RequestVerificationCommand, Result>
{
  public async Task<Result> Handle(RequestVerificationCommand request, CancellationToken cancellationToken)
  {
    var id = request.Id;
    var spec = new UserByIdSpec(id);
    var user = await _repository.SingleOrDefaultAsync(spec);
    if (user == null)
    {
      return UserErrors.NotFound(id);
    }
    user.GenerateEmailVerificationToken();
    return Result.Success();
  }
}
