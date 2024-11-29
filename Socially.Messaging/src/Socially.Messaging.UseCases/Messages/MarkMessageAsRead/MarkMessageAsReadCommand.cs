using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Socially.Messaging.UseCases.Messages.MarkMessageAsRead;
public record MarkMessageAsReadCommand(Guid MessageId) : ICommand<Result>;
