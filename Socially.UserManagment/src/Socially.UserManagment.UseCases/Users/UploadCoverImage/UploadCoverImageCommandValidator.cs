using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Socially.UserManagment.UseCases.Users.Common.DTOs;

namespace Socially.UserManagment.UseCases.Users.UploadCoverImage;
public class UploadCoverImageCommandValidator : AbstractValidator<UploadCoverImageCommand>
{
  private const long MaxAllowedInitialSize = 5 * 1024 * 1024;

  public UploadCoverImageCommandValidator()
  {
    RuleFor(x => x.File)
        .NotNull().WithMessage("Image file cannot be null.")
        .Must((imageFile) => imageFile.Length <= MaxAllowedInitialSize).WithMessage("Image size exceeds the maximum allowed size before compression (5 MB).");

  }


}
