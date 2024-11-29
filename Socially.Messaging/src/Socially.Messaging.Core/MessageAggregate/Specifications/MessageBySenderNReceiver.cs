using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Socially.Messaging.Core.MessageAggregate.Specifications;
public class MessageBySenderNReceiver : Specification<Message>
{
  public MessageBySenderNReceiver(Guid senderId, Guid receiverId)
  {
    Query.Where(m => m.ReceiverId == receiverId && m.SenderId == senderId).OrderBy(m => m.SentAt);
  }
}
