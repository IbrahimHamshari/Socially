using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Socially.Messaging.Core.MessageAggregate.Specifications;
public class MessageById: Specification<Message>, ISingleResultSpecification<Message>
{
  public MessageById(Guid id)
  {
    Query.Where(m => m.Id == id);
  }
}
