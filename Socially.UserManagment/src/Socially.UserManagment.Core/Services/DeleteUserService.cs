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
public class DeleteUserService(IRepository<User> _repository,
  IMediator _mediator,
  ILogger<DeleteUserService> _logger) : IDeleteUserService
{
  public async Task<Result> DeleteUser(Guid id)
  {
    _logger.LogInformation("Deleteing User {id}", id);
    User? userToDelete = await _repository.GetByIdAsync(id);
    if (userToDelete == null) return Result.NotFound();

    await _repository.DeleteAsync(userToDelete);

    var domainEvent = new UserDeletedEvent(userToDelete);
    await _mediator.Publish(domainEvent);
    // events to be added.
    return Result.Success();
  }
}
