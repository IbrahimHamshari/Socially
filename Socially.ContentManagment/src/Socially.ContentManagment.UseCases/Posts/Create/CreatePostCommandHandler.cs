using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Interfaces;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Posts.Create;
public class CreatePostCommandHandler(ICreatePostService _service) : ICommandHandler<CreatePostCommand, Result<PostDto>>
{
  public async Task<Result<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
  {
    // need to implement the uploading system.
    var mediaURL = "";
    return await _service.CreatePost(request.PostDTO.Userid, request.PostDTO.Content, request.PostDTO.Privacy, mediaURL);
  }
}
