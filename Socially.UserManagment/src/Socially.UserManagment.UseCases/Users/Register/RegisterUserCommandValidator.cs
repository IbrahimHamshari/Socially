using FluentValidation;
using Socially.UserManagment.Core.Constants;

namespace Socially.UserManagment.UseCases.Users.Register;
public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
  public RegisterUserCommandValidator()
  {
    RuleFor(x => x.User.Email).NotEmpty().WithMessage("Email is required").Matches(RegexConstants.EMAIL_REGEX).WithMessage("Email must be valid");
    RuleFor(x => x.User.FirstName).NotEmpty().WithMessage("Firstname is required").Matches(RegexConstants.NAME_REGEX).WithMessage("Name must be between 2 to 16 characters");
    RuleFor(x => x.User.LastName).NotEmpty().WithMessage("Firstname is required").Matches(RegexConstants.NAME_REGEX).WithMessage("Name must be between 2 to 16 characters");
    RuleFor(x => x.User.Password).NotEmpty().WithMessage("Password is required").Matches(RegexConstants.PASSWORD_REGEX).WithMessage("Password Must be between 8 to 24 characters, 1 capital letter, and 1 special character");
    RuleFor(x => x.User.Username).NotEmpty().WithMessage("Username is required").Matches(RegexConstants.USERNAME_REGEX).WithMessage("Username Must be between 6 to 18 characters");
  }
}
