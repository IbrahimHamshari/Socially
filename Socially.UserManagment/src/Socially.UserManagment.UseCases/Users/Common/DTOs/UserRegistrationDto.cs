﻿namespace Socially.UserManagment.UseCases.Users.Common.DTOs;

public class UserRegistrationDto
{
  public required string Username { get; set; }

  public required string Email { get; set; }

  public required string Password { get; set; }

  public required string FirstName { get; set; }

  public required string LastName { get; set; }

  public  required bool Gender { get; set; }
}
