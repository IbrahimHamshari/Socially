using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Messaging.Infrastructure.Interfaces;
public interface INotificationService
{
  Task NotifyUserAsync(Guid userId, object message);
  Task NotifyStatusUpdateAsync(Guid userId, Guid messageId, string status);

}
