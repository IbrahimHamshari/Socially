using FluentValidation;
using Socially.ContentManagment.Core.Constants;

namespace Socially.ContentManagment.UseCases.Users.ForgetPassword;

public class ForgetPasswordCommandValidator : AbstractValidator<ForgetPasswordCommand>
{
  public ForgetPasswordCommandValidator()
  {
    RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").Matches(RegexConstants.EMAIL_REGEX).WithMessage("Email must be valid");
  }
}
