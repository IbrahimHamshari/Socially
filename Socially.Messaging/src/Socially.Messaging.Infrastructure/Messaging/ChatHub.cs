using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Socially.Messaging.Infrastructure.Messaging;

[Authorize]
public class ChatHub : Hub
{
  public override async Task OnConnectedAsync()
  {
    await Clients.Client(Context.ConnectionId).SendAsync(Context.UserIdentifier ?? "nothing");
    await base.OnConnectedAsync();
  }
}
