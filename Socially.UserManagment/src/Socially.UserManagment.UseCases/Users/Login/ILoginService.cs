using Ardalis.Result;
using Socially.UserManagment.Core.UserAggregate;
using Socially.UserManagment.UseCases.Users.Common;

namespace Socially.UserManagment.UseCases.Users.Login;

public interface ILoginService
{
  Task<Result<Tokens>> LoginAsync(User user, string password);
}
