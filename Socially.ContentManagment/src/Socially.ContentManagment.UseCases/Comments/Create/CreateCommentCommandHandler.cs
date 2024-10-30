using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.PostAggregate.Errors;
using Socially.ContentManagment.Core.PostAggregate.Specifications;

namespace Socially.ContentManagment.UseCases.Comments.Create;
public class CreateCommentCommandHandler(IRepository<Post> _repository) : ICommandHandler<CreateCommentCommand, Result>
{
  public async Task<Result> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
  {
    var spec = new PostByIdSpec(request.createCommandDto.PostId);
    var post = await _repository.SingleOrDefaultAsync(spec, cancellationToken);
    if (post == null)
    {
      return PostErrors.NotFound(request.createCommandDto.PostId);
    }
    post.AddComment(request.userId,request.createCommandDto.Content);

    return Result.Success();
  }
}
