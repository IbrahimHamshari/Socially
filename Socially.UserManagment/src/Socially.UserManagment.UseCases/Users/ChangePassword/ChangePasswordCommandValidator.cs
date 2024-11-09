using FluentValidation;
using Socially.UserManagment.Core.Constants;

namespace Socially.UserManagment.UseCases.Users.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
  public ChangePasswordCommandValidator()
  {
    RuleFor(x => x.id).NotEmpty().WithMessage("Id is required");
    RuleFor(x => x.passwords.CurrentPassword).NotEmpty().WithMessage("Current Password is required").Matches(RegexConstants.PASSWORD_REGEX).WithMessage("Password Must be between 8 to 24 characters, 1 capital letter, and 1 special character");
    RuleFor(x => x.passwords.Password).NotEmpty().WithMessage("New Password is required").Matches(RegexConstants.PASSWORD_REGEX).WithMessage("Password Must be between 8 to 24 characters, 1 capital letter, and 1 special character");
  }
}
