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
  [Required, RegularExpression(RegexConstants.USERNAME_REGEX)]
  public required string Username { get; set; }

  [Required, EmailAddress]
  public required string Email { get; set; }

  [Required, RegularExpression(RegexConstants.PASSWORD_REGEX)]
  public required string Password { get; set; }

  [Required, RegularExpression(RegexConstants.NAME_REGEX)]
  public required string FirstName { get; set; }

  [Required, RegularExpression(RegexConstants.NAME_REGEX)]
  public required string LastName { get; set; }

  [Required]

  public required bool Gender { get; set; }
}
