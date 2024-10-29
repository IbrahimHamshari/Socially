using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using Socially.ContentManagment.Core.PostAggregate.Errors;
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
  public async Task<Result> DeletePost(Guid postId)
  {
    _logger.LogInformation("Deleting Contributor {contributorId}", postId);
    Post? aggregateToDelete = await _repository.GetByIdAsync(postId);
    if (aggregateToDelete == null) return PostErrors.NotFound(postId);

    await _repository.DeleteAsync(aggregateToDelete);
    return Result.Success();
  }
}
