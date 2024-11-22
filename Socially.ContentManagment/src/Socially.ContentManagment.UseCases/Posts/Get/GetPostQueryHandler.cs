using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.PostAggregate.Errors;
using Socially.ContentManagment.Core.PostAggregate.Specifications;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Posts.Get;
public class GetPostQueryHandler(IReadRepository<Post> _repository) : IQueryHandler<GetPostQuery, Result<PostDto>>
{
  public async Task<Result<PostDto>> Handle(GetPostQuery request, CancellationToken cancellationToken)
  {
    var id = request.id;
    var spec = new PostByIdSpec(request.id);
    
    var post = await _repository.SingleOrDefaultAsync(spec, cancellationToken);
    if(post == null)
    {
      return PostErrors.NotFound(request.id);
    }
    return Result.Success(new PostDto { Id = post.Id, Content = post.Content, Privacy = post.Privacy, MediaURL = post.MediaURL, UserId= post.UserId});
  }
}
