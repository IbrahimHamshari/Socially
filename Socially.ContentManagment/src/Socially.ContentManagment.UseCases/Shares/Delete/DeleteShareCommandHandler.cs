using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.PostAggregate.Errors;
using Socially.ContentManagment.Core.PostAggregate.Specifications;

namespace Socially.ContentManagment.UseCases.Shares.Delete;
public class DeleteShareCommandHandler(IRepository<Post> _repository) : ICommandHandler<DeleteShareCommand, Result>
{
  public async Task<Result> Handle(DeleteShareCommand request, CancellationToken cancellationToken)
  {
    var spec = new PostByIdSpec(request.sharePostDto.PostId);
    var post = await _repository.SingleOrDefaultAsync(spec, cancellationToken);
    if (post == null)
    {
      return PostErrors.NotFound(request.sharePostDto.PostId);
    }
    post.RemoveSharedPost(request.userId);

    return Result.Success();


  }
}
