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
public class CreateLikeCommandHandler(IRepository<Post> _repository) : ICommandHandler<CreateLikeCommand, Result>
{
  public async Task<Result> Handle(CreateLikeCommand request, CancellationToken cancellationToken)
  {
    var spec = new PostByIdSpec(request.createLikeDto.PostId);
    var post = await _repository.SingleOrDefaultAsync(spec, cancellationToken);
    if (post != null)
    {
      return PostErrors.NotFound(request.createLikeDto.PostId);
    }
    if (request.createLikeDto.CommentId != null)
    {
      post.LikeComment(request.createLikeDto.CommentId);
    }
    post.LikePost();
    return Result.Success();
  }
}
