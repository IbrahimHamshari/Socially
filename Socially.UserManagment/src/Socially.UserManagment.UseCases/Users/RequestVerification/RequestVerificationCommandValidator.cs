using FluentValidation;

namespace Socially.ContentManagment.UseCases.Users.RequestVerification;

public class RequestVerificationCommandValidator : AbstractValidator<RequestVerificationCommand>
{
  public RequestVerificationCommandValidator()
  {
    RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
  }
}
