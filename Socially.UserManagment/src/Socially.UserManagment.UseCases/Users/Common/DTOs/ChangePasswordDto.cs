using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

namespace Socially.UserManagment.UseCases.Users.Common.DTOs;
public class ChangePasswordDto(string currentPassowrd, string password)
{
  public string currentPassowrd { get; init; } = Guard.Against.InvalidPasswordFormat(currentPassowrd, nameof(currentPassowrd));
  public string password { get; init; } = Guard.Against.InvalidPasswordFormat(password, nameof(password));
}
