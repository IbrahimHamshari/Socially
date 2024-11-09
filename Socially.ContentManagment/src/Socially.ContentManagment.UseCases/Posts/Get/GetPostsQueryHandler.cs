using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.PostAggregate.Specifications;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Posts.Get;
public class GetPostsQueryHandler(IReadRepository<Post> _repository) : IQueryHandler<GetPostsQuery, Result<PostDto[]>>
{
  public async Task<Result<PostDto[]>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
  {
    var id = request.userId;
    var spec = new PostsByUserIdSpec(id);
    var posts = await _repository.ListAsync(spec, cancellationToken);
    return Result.Success(posts.Select(post => new PostDto { Id=post.Id, Content= post.Content, MediaURL = post.MediaURL, Privacy = post.Privacy, UserId = post.UserID}).ToArray());
  }
}
