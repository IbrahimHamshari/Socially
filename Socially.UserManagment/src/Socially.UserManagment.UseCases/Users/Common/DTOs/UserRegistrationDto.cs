using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Socially.UserManagment.Core.Constants;

namespace Socially.UserManagment.UseCases.Users.Common.DTOs;
public class UserRegistrationDto
{
  public required string Username { get; set; }

  public required string Email { get; set; }

  public required string Password { get; set; }

  public required string FirstName { get; set; }

  public required string LastName { get; set; }

  public required bool Gender { get; set; }
}
