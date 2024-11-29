using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.Messaging.UseCases.Messages.Common.DTOs;

namespace Socially.Messaging.UseCases.Messages.Read;
public record ReadMessageQuery(Guid SenderId, Guid ReceiverId) : IQuery<Result<List<MessageDto>?>>;
