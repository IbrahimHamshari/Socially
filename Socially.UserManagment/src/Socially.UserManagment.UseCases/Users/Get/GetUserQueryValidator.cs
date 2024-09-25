using FluentValidation;

namespace Socially.UserManagment.UseCases.Users.Get;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
  public GetUserQueryValidator()
  {
    RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
  }
}
