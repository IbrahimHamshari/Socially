using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

namespace Socially.UserManagment.UseCases.Users.Common.DTOs;
public class ChangePasswordDto(string currentPassword, string password)
{
  public string currentPassword { get; init; } = Guard.Against.InvalidPasswordFormat(currentPassword, nameof(currentPassword));
  public string password { get; init; } = Guard.Against.InvalidPasswordFormat(password, nameof(password));
}
