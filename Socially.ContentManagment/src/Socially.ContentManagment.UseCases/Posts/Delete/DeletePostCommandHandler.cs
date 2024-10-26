using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.PostAggregate.Errors;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Posts.Delete;
public class DeletePostCommandHandler(IRepository<Post> _repository) : ICommandHandler<DeletePostCommand, Result<PostDto>>
{

  public async Task<Result<PostDto>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
  {
    Guid id = request.id;
    var post = await _repository.GetByIdAsync(id);
    if (post == null)
    {
      return PostErrors.NotFound(id);
    }
    await _repository.DeleteAsync(post);
    return Result.Success(new PostDto { Id = post.Id, Content = post.Content, Privacy = post.Privacy, MediaURL = post.MediaURL, UserId = post.UserID});
  }
}
