using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Socially.Messaging.Core.MessageAggregate;

namespace Socially.Messaging.UseCases.Messages.Common.DTOs;
public class MessageDto
{
  public Guid Id {  get; set; }

  public required string Content { get; set; }

  public MessageStatus Status { get; set; }


}
