using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.Messaging.Core.MessageAggregate;
using Socially.Messaging.Core.MessageAggregate.Specifications;
using Socially.Messaging.UseCases.Messages.Common.DTOs;

namespace Socially.Messaging.UseCases.Messages.Read;
public class ReadMessageQueryHandler(IReadRepository<Message> _repository) : IQueryHandler<ReadMessageQuery, Result<List<MessageDto>?>>
{
  public async Task<Result<List<MessageDto>?>> Handle(ReadMessageQuery request, CancellationToken cancellationToken)
  {
    var spec = new MessageBySenderNReceiver(request.SenderId, request.ReceiverId);
    var messages = await _repository.ListAsync(spec, cancellationToken);
    List<MessageDto>? result = messages.Select(m => new MessageDto { Id = m.Id, Content = m.Content, Status = m.Status }).ToList();
    return Result<List<MessageDto>?>.Success(result);
  }
}
