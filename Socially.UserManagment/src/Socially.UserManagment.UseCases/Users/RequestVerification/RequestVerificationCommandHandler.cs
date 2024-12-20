﻿using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.UserManagment.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Errors;
using Socially.UserManagment.Core.UserAggregate.Specifications;

namespace Socially.UserManagment.UseCases.Users.RequestVerification;

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
