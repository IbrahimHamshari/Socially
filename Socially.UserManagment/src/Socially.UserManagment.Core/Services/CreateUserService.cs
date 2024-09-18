using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.Core.Interfaces;
using Socially.UserManagment.Core.UserAggregate.Events;

namespace Socially.UserManagment.Core.Services;
public class CreateUserService(IRepository<User> _repository,
  IMediator _mediator,
  ILogger<CreateUserService> _logger) : ICreateUserService
{
  public async Task<Result<Guid>> CreateUser(User user)
  {
    _logger.LogInformation("Creating A new User with an email of {email}", user.Email);
    var createdUser = await _repository.AddAsync(user);

    var domainEvent = new UserCreatedEvent(createdUser);
    await _mediator.Publish(domainEvent);
    return Result.Success(createdUser.Id);
  }
}
