using FluentValidation;

namespace Socially.UserManagment.UseCases.Users.RequestVerification;

public class RequestVerificationCommandValidator : AbstractValidator<RequestVerificationCommand>
{
  public RequestVerificationCommandValidator()
  {
    RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
  }
}
