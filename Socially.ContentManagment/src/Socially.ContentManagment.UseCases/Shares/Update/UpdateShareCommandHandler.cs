using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.PostAggregate.Errors;
using Socially.ContentManagment.Core.PostAggregate.Specifications;

namespace Socially.ContentManagment.UseCases.Shares.Update;
public class UpdateShareCommandHandler(IRepository<Post> _repository) : ICommandHandler<UpdateShareCommand, Result>
{
  public async Task<Result> Handle(UpdateShareCommand request, CancellationToken cancellationToken)
  {
    var spec = new PostByIdSpec(request.sharePostDto.PostId);
    var post = await _repository.SingleOrDefaultAsync(spec, cancellationToken);
    if(post == null)
    {
      return PostErrors.NotFound(request.sharePostDto.PostId);
    }
    post.UpdateShareMessage(, request.sharePostDto.Message);
    return Result.Success();
  }
}
