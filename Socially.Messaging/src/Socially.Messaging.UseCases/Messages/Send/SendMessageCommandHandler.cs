using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.Messaging.Core.MessageAggregate;
using Socially.Messaging.Infrastructure.Interfaces;
using Socially.Messaging.UseCases.Messages.Common.DTOs;

namespace Socially.Messaging.UseCases.Messages.Send;
public class SendMessageCommandHandler(IRepository<Message> _repoistory, INotificationService _notficiationService) : ICommandHandler<SendMessageCommand, Result<Guid>>
{
  public async Task<Result<Guid>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
  {
    var message = new Message(request.sendMessageDto.Content, request.SenderId, request.sendMessageDto.ReceiverId);

    await _repoistory.AddAsync(message);

    await _notficiationService.NotifyUserAsync(request.sendMessageDto.ReceiverId, new MessageDto { Content = message.Content, Id = message.Id, Status = message.Status});

    return message.Id;
  }
}
