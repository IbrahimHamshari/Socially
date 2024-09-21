using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Socially.UserManagment.Core.Constants;

namespace Socially.UserManagment.UseCases.Users.Common.DTOs;
public class UserLoginDto
{
  [Required, RegularExpression(RegexConstants.USERNAME_REGEX)]
  public required string UserName { get; set; }
  [Required, RegularExpression(RegexConstants.PASSWORD_REGEX)]
  public required string Password { get; set; }
}
