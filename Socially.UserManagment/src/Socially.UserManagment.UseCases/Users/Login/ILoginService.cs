using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Socially.UserManagement.Core.UserAggregate;

namespace Socially.UserManagment.UseCases.Users.Login;
public interface ILoginService
{
  Task<Result<string[]>> LoginAsync(User user , string password);

}
