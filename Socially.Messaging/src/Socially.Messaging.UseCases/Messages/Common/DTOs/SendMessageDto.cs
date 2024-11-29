using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Messaging.UseCases.Messages.Common.DTOs;
public class SendMessageDto
{
  public required string Content { get; set; }
  public required Guid ReceiverId { get; set; }
}
