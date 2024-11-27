using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SharedKernel;

namespace Socially.Messaging.Core.MessageAggregate;
public class Message : EntityBase<Guid>, IAggregateRoot
{
  public string Content { get; private set; }
  public DateTime SentAt { get; private set; }
  public Guid SenderId { get; private set; }
  public Guid ReceiverId { get; private set; }
  public MessageStatus Status { get; private set; }

  public Message(string content, Guid senderId, Guid receiverId)
  {
    Id = Guid.NewGuid();
    Content = content ?? throw new ArgumentNullException(nameof(content));
    SentAt = DateTime.UtcNow;
    SenderId = senderId;
    ReceiverId = receiverId;
    Status = MessageStatus.Pending;
  }

  public void MarkAsRead()
  {
    if (Status == MessageStatus.Read)
      throw new InvalidOperationException("Message is already marked as read.");

    Status = MessageStatus.Read;
  }

  public void MarkAsDelivered()
  {
    if (Status == MessageStatus.Delivered)
      throw new InvalidOperationException("Message is already marked as delivered.");

    Status = MessageStatus.Delivered;
  }

}
