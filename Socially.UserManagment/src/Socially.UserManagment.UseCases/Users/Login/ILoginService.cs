using Ardalis.Result;
using Socially.ContentManagment.Core.UserAggregate;
using Socially.ContentManagment.UseCases.Users.Common;

namespace Socially.ContentManagment.UseCases.Users.Login;

public interface ILoginService
{
  Task<Result<Tokens>> LoginAsync(User user, string password);
}
