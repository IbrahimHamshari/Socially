using FluentValidation;
using Socially.UserManagment.Core.Constants;
using Socially.UserManagment.UseCases.Users.ChangePasswordForget;

namespace Socially.UserManagment.UseCases.Users.ChangeForgetPassword;

public class ChangeForgetPasswordCommandValidator : AbstractValidator<ChangeForgetPasswordCommand>
{
  public ChangeForgetPasswordCommandValidator()
  {
    RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required");
    RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Password is required").Matches(RegexConstants.PASSWORD_REGEX).WithMessage("Password Must be between 8 to 24 characters, 1 capital letter, and 1 special character");
  }
}
