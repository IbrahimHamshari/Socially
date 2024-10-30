using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Socially.ContentManagment.UseCases.Constants;
using Socially.ContentManagment.UseCases.Interfaces;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;
using Socially.ContentManagment.UseCases.Posts.Utils;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

using static System.Net.Mime.MediaTypeNames;

namespace Socially.ContentManagment.UseCases.Posts.Create;

public class CreatePostCommandHandler(IFileStorageService _fileStorage, ICreatePostService _service)
    : ICommandHandler<CreatePostCommand, Result<PostDto>>
{
  public async Task<Result<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
  {
    var mediaURL = "";

    if (request.PostDTO.Media != null)
    {
      var media = request.PostDTO.Media;

      // Check if the media is an image or video by examining the content type
      if (media.ContentType.StartsWith("image"))
      {
        // Compress and upload image
        using var compressedImageStream = await MediaCompression.CompressImageAsync(media.OpenReadStream());
        mediaURL = await _fileStorage.UploadFileAsync(compressedImageStream,Guid.NewGuid().ToString("N"), BucketStorageConstants.POSTMEDIABUCKET);
      }
      else if (media.ContentType.StartsWith("video"))
      {
        // Compress and upload video
        using var compressedVideoStream = await MediaCompression.CompressVideoAsync(media.OpenReadStream());
        mediaURL = await _fileStorage.UploadFileAsync(compressedVideoStream, Guid.NewGuid().ToString("N"), BucketStorageConstants.POSTMEDIABUCKET);
      }
    }

    return await _service.CreatePost(request.UserId, request.PostDTO.Content, request.PostDTO.Privacy, mediaURL);
  }

  

}
