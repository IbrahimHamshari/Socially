using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.PostAggregate.Errors;
using Socially.ContentManagment.Core.PostAggregate.Specifications;

namespace Socially.ContentManagment.UseCases.Comments.Update;
public class UpdateCommentCommandHandler(IRepository<Post> _repository) : ICommandHandler<UpdateCommentCommand, Result>
{
  public async Task<Result> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
  {
    var spec = new PostByIdSpec(request.updateCommentDto.PostId);
    var post = await _repository.SingleOrDefaultAsync(spec,cancellationToken);
    if(post == null)
    {
      return PostErrors.NotFound(request.updateCommentDto.Id);
    }
    post.UpdateComment(request.updateCommentDto.Id, request.updateCommentDto.Content);

    return Result.Success();

  }
}
