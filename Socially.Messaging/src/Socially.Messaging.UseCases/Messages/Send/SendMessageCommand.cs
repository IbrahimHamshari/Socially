using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.Messaging.UseCases.Messages.Common.DTOs;

namespace Socially.Messaging.UseCases.Messages.Send;

  public record SendMessageCommand(SendMessageDto sendMessageDto, Guid SenderId) : ICommand<Result<Guid>>;
