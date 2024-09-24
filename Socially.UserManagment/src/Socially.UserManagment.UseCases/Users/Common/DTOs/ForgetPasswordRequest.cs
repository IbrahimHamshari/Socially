using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

namespace Socially.UserManagment.UseCases.Users.Common.DTOs;
public class ForgetPasswordRequest(string email)
{
  public string Email { get; init; } = email;
}
