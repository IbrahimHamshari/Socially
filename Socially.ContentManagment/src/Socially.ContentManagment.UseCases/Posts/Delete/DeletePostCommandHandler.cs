using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.PostAggregate.Errors;
using Socially.ContentManagment.UseCases.Interfaces;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Posts.Delete;
public class DeletePostCommandHandler(IDeletePostService _service) : ICommandHandler<DeletePostCommand, Result<PostDto>>
{

  public async Task<Result<PostDto>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
  {
    return await _service.DeletePost(request.Id, request.UserId, cancellationToken);
  }
}
