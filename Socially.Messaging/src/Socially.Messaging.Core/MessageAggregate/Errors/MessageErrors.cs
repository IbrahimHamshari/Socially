using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;

namespace Socially.Messaging.Core.MessageAggregate.Errors;
public static class MessageErrors
{
  public static Result NotFound(Guid messageId) => Result.NotFound("Messages.NotFound", $"The message with the Id = '{messageId}' was not found");
}
