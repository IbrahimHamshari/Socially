using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

namespace Socially.UserManagment.UseCases.Users.Common.DTOs;
public class ChangeForgetPasswordRequest(string password)
{
  public string Password { get; set; } = password;
}
