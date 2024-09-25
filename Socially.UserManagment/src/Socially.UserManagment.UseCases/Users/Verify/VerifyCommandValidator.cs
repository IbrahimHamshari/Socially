using FluentValidation;

namespace Socially.UserManagment.UseCases.Users.Verify;

public class VerifyCommandValidator : AbstractValidator<VerifyCommand>
{
  public VerifyCommandValidator()
  {
    RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required");
  }
}
