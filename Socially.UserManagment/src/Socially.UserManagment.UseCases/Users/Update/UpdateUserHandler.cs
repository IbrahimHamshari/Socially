using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.UseCases.Users.Common.DTOs;

namespace Socially.UserManagment.UseCases.Users.Update;
public class UpdateUserHandler(IRepository<User> _repository) : ICommandHandler<UpdateUserCommand, Result<UserUpdateDto>>
{
  public async Task<Result<UserUpdateDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
  {
    var user = await _repository.GetByIdAsync(request.Id, cancellationToken);
    if (user == null)
    {
      return Result.NotFound("User not found.");
    }

    // Update the user's properties using domain methods
    if (!string.IsNullOrWhiteSpace(request.UserDto.Email))
    {
      user.UpdateEmail(request.UserDto.Email);
    }

    if (!string.IsNullOrWhiteSpace(request.UserDto.FirstName))
    {
      user.UpdateFirstName(request.UserDto.FirstName);
    }

    if (!string.IsNullOrWhiteSpace(request.UserDto.LastName))
    {
      user.UpdateLastName(request.UserDto.LastName);
    }

    if (!string.IsNullOrWhiteSpace(request.UserDto.Bio))
    {
      user.UpdateBio(request.UserDto.Bio);
    }

    if (!string.IsNullOrWhiteSpace(request.UserDto.ProfilePictureURL))
    {
      user.UpdateProfilePictureURL(request.UserDto.ProfilePictureURL);
    }

    if (!string.IsNullOrWhiteSpace(request.UserDto.CoverPhotoURL))
    {
      user.UpdateCoverPhotoURL(request.UserDto.CoverPhotoURL);
    }

    if (request.UserDto.DateOfBirth.HasValue)
    {
      user.UpdateDateOfBirth(request.UserDto.DateOfBirth.Value);
    }

    if(request.UserDto.Gender.HasValue)
    {
      user.UpdateGender(request.UserDto.Gender.Value);
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
