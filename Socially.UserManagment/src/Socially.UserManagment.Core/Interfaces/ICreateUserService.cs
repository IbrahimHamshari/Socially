using Ardalis.Result;
using Socially.UserManagment.Core.UserAggregate;

namespace Socially.UserManagment.Core.Interfaces;

public interface ICreateUserService
{
  public Task<Result<Guid>> CreateUser(User user);
}
