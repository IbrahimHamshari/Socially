using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Socially.UserManagment.UseCases.Users.RequestVerification;
public class RequestVerificationCommandValidator : AbstractValidator<RequestVerificationCommand>
{
  public RequestVerificationCommandValidator()
  {
    RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
  }
}
