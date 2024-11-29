using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Socially.Messaging.Infrastructure.Interfaces;

namespace Socially.Messaging.Infrastructure.Messaging.Services;
public class SignalRNotificationService : INotificationService
{
  private readonly IHubContext<ChatHub> _hubContext;

  public SignalRNotificationService(IHubContext<ChatHub> hubContext)
  {
    _hubContext = hubContext;
  }

  public async Task NotifyUserAsync(Guid userId, object message)
  {
    await _hubContext.Clients.User(userId.ToString())
        .SendAsync("ReceiveMessage", message);
  }

  public async Task NotifyStatusUpdateAsync(Guid userId, Guid messageId, string status)
  {
    await _hubContext.Clients.User(userId.ToString())
        .SendAsync("MessageStatusUpdate", new { messageId, status });
  }
}

