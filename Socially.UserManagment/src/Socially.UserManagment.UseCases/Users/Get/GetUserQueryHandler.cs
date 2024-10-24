using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.UserAggregate;
using Socially.ContentManagment.Core.UserAggregate.Errors;
using Socially.ContentManagment.Core.UserAggregate.Specifications;
using Socially.ContentManagment.UseCases.Users.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Users.Get;

public class GetUserQueryHandler(IReadRepository<User> _repository) : IQueryHandler<GetUserQuery, Result<UserDto>>
{
  public async Task<Result<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
  {
    var id = request.Id;
    var spec = new UserByIdSpec(id);
    var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null) return UserErrors.NotFound(id);
    return new UserDto { Id = entity.Id, Bio = entity.Bio, Email = entity.Email, FirstName = entity.FirstName, Gender = entity.Gender, LastName = entity.LastName, CoverPhotoURL = entity.CoverPhotoURL, DateOfBirth = entity.DateOfBirth, ProfilePictureURL = entity.ProfilePictureURL };
  }
}
