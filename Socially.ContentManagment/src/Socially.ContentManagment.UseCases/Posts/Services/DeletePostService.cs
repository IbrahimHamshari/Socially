using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using Socially.ContentManagment.Core.PostAggregate.Errors;
using Socially.ContentManagment.Core.PostAggregate.Specifications;
using Socially.ContentManagment.UseCases.Interfaces;

namespace Socially.ContentManagment.UseCases.Services;
/// <summary>
/// This is here mainly so there's an example of a domain service
/// and also to demonstrate how to fire domain events from a service.
/// </summary>
/// <param name="_repository"></param>
/// <param name="_mediator"></param>
/// <param name="_logger"></param>
public class DeletePostService(IRepository<Post> _repository,
  ILogger<DeletePostService> _logger) : IDeletePostService
{
  public async Task<Result> DeletePost(Guid postId, Guid userId, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Deleting Contributor {contributorId}", postId);
    var spec = new PostByIdAndUserId(postId,userId);
    var aggregateToDelete = await _repository.SingleOrDefaultAsync(spec,cancellationToken);
    if (aggregateToDelete == null) return PostErrors.NotFound(postId);
    
    await _repository.DeleteAsync(aggregateToDelete);
    return Result.Success();
  }
}
