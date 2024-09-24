using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

namespace Socially.UserManagment.UseCases.Users.Common.DTOs;
public class ChangePasswordDto(string currentPassword, string password)
{
  public string currentPassword { get; init; } = currentPassword;
  public string password { get; init; } = password;
}
