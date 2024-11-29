using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using Socially.Messaging.Core.MessageAggregate;
using Socially.Messaging.Core.MessageAggregate.Errors;
using Socially.Messaging.Core.MessageAggregate.Specifications;
using Socially.Messaging.Infrastructure.Interfaces;

namespace Socially.Messaging.UseCases.Messages.MarkMessageAsRead;
public class MarkMessageAsReadCommandHandler(IRepository<Message> _repository, INotificationService _notificationService) : ICommandHandler<MarkMessageAsReadCommand, Result>
{
  public async Task<Result> Handle(MarkMessageAsReadCommand request, CancellationToken cancellationToken)
  {
    var spec = new MessageById(request.MessageId);
    var message = await _repository.SingleOrDefaultAsync(spec, cancellationToken);
    if (message == null)
      return MessageErrors.NotFound(request.MessageId);

    message.MarkAsRead();
    await _repository.SaveChangesAsync(cancellationToken);

    // Notify the sender about the read status
    await _notificationService.NotifyStatusUpdateAsync(message.SenderId, message.Id, "Read");

    return Result.Success();

  }
}
