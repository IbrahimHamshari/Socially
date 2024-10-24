using Ardalis.Result;
using Socially.ContentManagment.Core.UserAggregate;

namespace Socially.ContentManagment.Core.Interfaces;

public interface ICreateUserService
{
  public Task<Result<Guid>> CreateUser(User user);
}
