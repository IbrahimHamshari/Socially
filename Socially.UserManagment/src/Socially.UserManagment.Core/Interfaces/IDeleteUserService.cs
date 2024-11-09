using Ardalis.Result;

namespace Socially.UserManagment.Core.Interfaces;

public interface IDeleteUserService
{
  public Task<Result> DeleteUser(Guid id);
}
