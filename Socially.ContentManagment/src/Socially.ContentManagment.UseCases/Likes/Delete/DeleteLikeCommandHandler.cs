using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.PostAggregate.Errors;
using Socially.ContentManagment.Core.PostAggregate.Specifications;

namespace Socially.ContentManagment.UseCases.Likes.Delete;
public class DeleteLikeCommandHandler(IRepository<Post> _repository) : ICommandHandler<DeleteLikeCommand, Result>
{
  public async Task<Result> Handle(DeleteLikeCommand request, CancellationToken cancellationToken)
  {
    var spec = new PostByIdSpec(request.deleteLikeDto.PostId);
    var post = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (post != null)
    {
      return PostErrors.NotFound(post.Id);
    }
    post.LikeComment(request.deleteLikeDto.CommentId);

  }
}
