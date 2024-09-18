using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Socially.UserManagement.Core.UserAggregate;

namespace Socially.UserManagment.Core.Interfaces;
internal interface ICreateUserService
{
  public Task<Result<Guid>> CreateUser(User user);
}
