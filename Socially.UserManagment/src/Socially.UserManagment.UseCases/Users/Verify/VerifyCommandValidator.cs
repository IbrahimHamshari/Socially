using FluentValidation;

namespace Socially.ContentManagment.UseCases.Users.Verify;

public class VerifyCommandValidator : AbstractValidator<VerifyCommand>
{
  public VerifyCommandValidator()
  {
    RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required");
  }
}
