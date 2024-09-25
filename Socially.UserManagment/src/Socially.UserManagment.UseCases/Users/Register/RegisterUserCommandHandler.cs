﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;
using Socially.UserManagement.Core.UserAggregate;

namespace Socially.UserManagment.UseCases.Users.Register;
public class RegisterUserCommandHandler(IRepository<User> _repository  ) : ICommandHandler<RegisterUserCommand, Result<Guid>>
{
  public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
  {
    var newUser = new User(request.User.Username, request.User.Email, request.User.Password, request.User.FirstName, request.User.LastName, request.User.Gender);
    newUser.GenerateEmailVerificationToken();
    var User = await _repository.AddAsync(newUser);
    return Result.Created(newUser.Id);
  }

}