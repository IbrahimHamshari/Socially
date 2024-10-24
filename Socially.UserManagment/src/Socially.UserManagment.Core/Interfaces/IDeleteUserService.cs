using Ardalis.Result;

namespace Socially.ContentManagment.Core.Interfaces;

public interface IDeleteUserService
{
  public Task<Result> DeleteUser(Guid id);
}
