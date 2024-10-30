using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.PostAggregate.Errors;
using Socially.ContentManagment.Core.PostAggregate.Specifications;

namespace Socially.ContentManagment.UseCases.Comments.Delete;
public class DeleteCommentCommandHandler(IRepository<Post> _repository) : ICommandHandler<DeleteCommentCommand, Result>
{
  public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
  {
    var spec = new PostByIdSpec(request.deleteCommentDto.Id);
    var post = await _repository.SingleOrDefaultAsync(spec);
    if (post == null)
    {
      return PostErrors.NotFound(request.deleteCommentDto.Id);
    }
    post.DeleteComment(request.userId,request.deleteCommentDto.Id);
    return Result.Success();
  }
}
