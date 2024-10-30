using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.PostAggregate.Errors;
using Socially.ContentManagment.Core.PostAggregate.Specifications;

namespace Socially.ContentManagment.UseCases.Likes.Create;
public class ToggleLikeCommandHandler(IRepository<Post> _repository) : ICommandHandler<ToggleLikeCommand, Result>
{
  public async Task<Result> Handle(ToggleLikeCommand request, CancellationToken cancellationToken)
  {
    var spec = new PostByIdSpec(request.createLikeDto.PostId);
    var post = await _repository.SingleOrDefaultAsync(spec, cancellationToken);
    if (post == null)
    {
      return PostErrors.NotFound(request.createLikeDto.PostId);
    }
    if (request.createLikeDto.CommentId != null)
    {
      post.LikeComment(request.createLikeDto.CommentId.Value, request.userId);
    }
    else
    {
    post.LikePost(request.userId);
    }
    return Result.Success();
  }
}
