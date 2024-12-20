﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using System.IO.Compression;
using Socially.UserManagment.UseCases.Users.Common;
using Socially.UserManagment.UseCases.Interfaces;
using Socially.UserManagment.Infrastructure.Constants;
using Socially.UserManagment.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Specifications;
using Socially.UserManagment.Core.UserAggregate.Errors;
namespace Socially.UserManagment.UseCases.Users.UploadCoverImage;
public class UploadCoverImageCommandHandler(IFileStorageService _fileStorage, IRepository<User> _repository) : ICommandHandler<UploadCoverImageCommand, Result<string>>
{
  public async Task<Result<string>> Handle(UploadCoverImageCommand request, CancellationToken cancellationToken)
  {
    var image = request.File;
    var id = request.Id;
    var compressImage = await ImageProcessing.CompressImage(image,1500,500);
    using var stream = image.OpenReadStream();
    var coverPhoto = await _fileStorage.UploadFileAsync(stream, id.ToString(), BucketStorageConstants.COVERPHOTOBUCKET);
    var spec = new UserByIdSpec(id);
    var user = await _repository.SingleOrDefaultAsync(spec);
    if (user == null)
    {
      return UserErrors.NotFound(id);
    }
    user.UpdateCoverPhotoURL(coverPhoto);
    await _repository.SaveChangesAsync();
    return Result.Success(coverPhoto);

  }
}
