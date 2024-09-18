using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;

namespace Socially.UserManagment.Core.Interfaces;
public interface IDeleteUserService
{
  public Task<Result> DeleteUser(Guid id);
}
