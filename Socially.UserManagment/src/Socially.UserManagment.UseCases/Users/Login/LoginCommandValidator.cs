using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Socially.UserManagment.Core.Constants;

namespace Socially.UserManagment.UseCases.Users.Login;
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
  public LoginCommandValidator()
  {
    RuleFor(x => x.User.Username).NotEmpty().WithMessage("Username is required").Matches(RegexConstants.USERNAME_REGEX).WithMessage("Username Must be between 6 to 18 characters");
    RuleFor(x => x.User.Password).NotEmpty().WithMessage("Password is required").Matches(RegexConstants.PASSWORD_REGEX).WithMessage("Password Must be between 8 to 24 characters, 1 capital letter, and 1 special character");
  }
}
