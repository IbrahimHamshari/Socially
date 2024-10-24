using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;
using Socially.ContentManagment.Core.UserAggregate;
using Socially.ContentManagment.Core.UserAggregate.Errors;
using Socially.ContentManagment.Core.UserAggregate.Specifications;

namespace Socially.ContentManagment.UseCases.Users.ChangePassword;

public class ChangePasswordCommandHandler(IRepository<User> _repository,
  ILogger<ChangePasswordCommandHandler> _logger) : ICommandHandler<ChangePasswordCommand, Result>
{
  public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
  {
    var currentPassowrd = request.passwords.CurrentPassword;
    var newPassword = request.passwords.Password;
    var id = request.id;
    var spec = new UserByIdSpec(id);
    var user = await _repository.SingleOrDefaultAsync(spec, cancellationToken);
    if (user == null)
    {
      return UserErrors.NotFound(id);
    }
    user.ChangePassword(currentPassowrd, newPassword);
    await _repository.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("Password Has Changed for the user with Id of {id}", user.Id);
    return Result.Success();
  }
}
