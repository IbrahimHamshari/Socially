using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Socially.UserManagment.Core.Constants;

namespace Socially.UserManagment.UseCases.Users.Update;
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
  public UpdateUserCommandValidator()
  {
    RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    RuleFor(x => x.User.Email).Matches(RegexConstants.EMAIL_REGEX).WithMessage("Email must be valid");
    RuleFor(x => x.User.FirstName).Matches(RegexConstants.NAME_REGEX).WithMessage("Name must be between 2 to 16 characters");
    RuleFor(x => x.User.LastName).Matches(RegexConstants.NAME_REGEX).WithMessage("Name must be between 2 to 16 characters");
    

  }
}
