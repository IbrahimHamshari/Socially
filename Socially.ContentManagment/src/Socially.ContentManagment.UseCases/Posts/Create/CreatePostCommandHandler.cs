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
using Socially.ContentManagment.UseCases.Posts.Services;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

using static System.Net.Mime.MediaTypeNames;

namespace Socially.ContentManagment.UseCases.Posts.Create;

public class CreatePostCommandHandler(IMediaUploadService _mediaService, ICreatePostService _service)
    : ICommandHandler<CreatePostCommand, Result<PostDto>>
{
  public async Task<Result<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
  {
    var mediaURL = await _mediaService.UploadMediaAsync(request.PostDTO.Media);

    return await _service.CreatePost(request.UserId, request.PostDTO.Content, request.PostDTO.Privacy, mediaURL);
  }

  

}
