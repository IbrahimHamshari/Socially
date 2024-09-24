using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Errors;
using Socially.UserManagment.UseCases.Users.Common.DTOs;

namespace Socially.UserManagment.UseCases.Users.Update;
public class UpdateUserCommandHandler(IRepository<User> _repository) : ICommandHandler<UpdateUserCommand, Result<UserUpdateDto>>
{
  public async Task<Result<UserUpdateDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
  {
    var id = request.Id;
    var user = await _repository.GetByIdAsync(id, cancellationToken);
    if (user == null)
    {
      return UserErrors.NotFound(id);
    }

    // Update the user's properties using domain methods
    if (!string.IsNullOrWhiteSpace(request.User.Email))
    {
      user.UpdateEmail(request.User.Email);
    }

    if (!string.IsNullOrWhiteSpace(request.User.FirstName))
    {
      user.UpdateFirstName(request.User.FirstName);
    }

    if (!string.IsNullOrWhiteSpace(request.User.LastName))
    {
      user.UpdateLastName(request.User.LastName);
    }

    if (!string.IsNullOrWhiteSpace(request.User.Bio))
    {
      user.UpdateBio(request.User.Bio);
    }

    if (!string.IsNullOrWhiteSpace(request.User.ProfilePictureURL))
    {
      user.UpdateProfilePictureURL(request.User.ProfilePictureURL);
    }

    if (!string.IsNullOrWhiteSpace(request.User.CoverPhotoURL))
    {
      user.UpdateCoverPhotoURL(request.User.CoverPhotoURL);
    }

    if (request.User.DateOfBirth.HasValue)
    {
      user.UpdateDateOfBirth(request.User.DateOfBirth.Value);
    }

    if (request.User.Gender.HasValue)
    {
      user.UpdateGender(request.User.Gender.Value);
    }

    // Save the updated user to the repository
    await _repository.SaveChangesAsync(cancellationToken);

    // Map the updated user to a DTO and return it
    var userDto = new UserUpdateDto
    {
      Id = request.Id,
      Email = user.Email,
      FirstName = user.FirstName,
      LastName = user.LastName,
      Bio = user.Bio,
      ProfilePictureURL = user.ProfilePictureURL,
      CoverPhotoURL = user.CoverPhotoURL,
      DateOfBirth = user.DateOfBirth,
      Gender = user.Gender
    };

    return Result.Success(userDto);
  }
}
