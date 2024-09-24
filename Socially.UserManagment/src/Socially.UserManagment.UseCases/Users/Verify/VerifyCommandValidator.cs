using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Socially.UserManagment.UseCases.Users.Verify;
public class VerifyCommandValidator : AbstractValidator<VerifyCommand>
{
  public VerifyCommandValidator()
  {
    RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required");
  }
}
