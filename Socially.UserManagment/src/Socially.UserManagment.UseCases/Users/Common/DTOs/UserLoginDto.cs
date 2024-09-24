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
  public required string Username { get; set; }
  public required string Password { get; set; }
}
