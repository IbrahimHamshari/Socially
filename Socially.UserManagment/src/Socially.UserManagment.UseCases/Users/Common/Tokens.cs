using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.UserManagment.UseCases.Users.Common;
public class Tokens
{
  public required string AccessToken { get; init; }
  public required string RefreshToken { get; init; }
}
