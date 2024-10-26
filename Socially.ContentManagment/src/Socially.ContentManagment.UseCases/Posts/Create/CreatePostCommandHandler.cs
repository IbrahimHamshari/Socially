using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Posts.Create;
public class CreatePostCommandHandler(IRepository<Post> _repository) : ICommandHandler<CreatePostCommand, Result<PostDto>>
{
  public async Task<Result<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
  {
    var post = new Post(Guid.NewGuid(), request.PostDTO.Userid, request.PostDTO.Content , request.PostDTO.Privacy, request.PostDTO.MediaURL);
    var newPost = await _repository.AddAsync(post);
    return Result.Created(new PostDto { Id = post.Id, Content = post.Content, Privacy = post.Privacy, MediaURL = post.MediaURL, UserId = post.UserID});
  }
}
