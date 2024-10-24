using FluentValidation;

namespace Socially.ContentManagment.UseCases.Users.Refresh;

public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
  public RefreshCommandValidator()
  {
    RuleFor(x => x.refreshToken).NotEmpty().WithMessage("Refresh Token is required");
  }
}
