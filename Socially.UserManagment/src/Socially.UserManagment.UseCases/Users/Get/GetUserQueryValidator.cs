using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Socially.UserManagment.UseCases.Users.Get;
public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
  public GetUserQueryValidator()
  {
    RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
  }
}
