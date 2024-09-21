using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.UseCases.Users.Common;

namespace Socially.UserManagment.UseCases.Users.Login;
public interface ILoginService
{
  Task<Result<Tokens>> LoginAsync(User user , string password);

}
