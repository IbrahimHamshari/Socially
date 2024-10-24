using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.UserAggregate;
using Socially.ContentManagment.Core.UserAggregate.Errors;
using Socially.ContentManagment.Core.UserAggregate.Specifications;
using Socially.ContentManagment.Infrastructure.Constants;
using Socially.ContentManagment.UseCases.Interfaces;
using Socially.ContentManagment.UseCases.Users.Common;

namespace Socially.ContentManagment.UseCases.Users.UploadProfilePicture;
public class UploadProfilePictureCommandHandler(IFileStorageService _fileStorage, IRepository<User> _repository) : ICommandHandler<UploadProfilePictureCommand, Result<string>>
{
  public async Task<Result<string>> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
  {
    var image = request.File;
    var id = request.Id;
    var compressImage = await ImageProcessing.CompressImage(image, 400,400);
    using var stream = image.OpenReadStream();
    var coverPhoto = await _fileStorage.UploadFileAsync(stream, id.ToString(), BucketStorageConstants.COVERPHOTOBUCKET);
    var spec = new UserByIdSpec(id);
    var user = await _repository.SingleOrDefaultAsync(spec);
    if (user == null)
    {
      return UserErrors.NotFound(id);
    }
    user.UpdateProfilePictureURL(coverPhoto);
    await _repository.SaveChangesAsync();
    return Result.Success(coverPhoto);
  }
}
