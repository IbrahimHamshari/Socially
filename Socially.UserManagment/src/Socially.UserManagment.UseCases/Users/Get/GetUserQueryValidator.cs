using FluentValidation;

namespace Socially.ContentManagment.UseCases.Users.Get;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
  public GetUserQueryValidator()
  {
    RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
  }
}
