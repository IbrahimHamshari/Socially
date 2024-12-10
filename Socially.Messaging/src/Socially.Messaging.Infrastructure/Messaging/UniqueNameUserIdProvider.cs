using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Socially.Messaging.Infrastructure.Messaging;
public class UniqueNameUserIdProvider : IUserIdProvider
{
  public string? GetUserId(HubConnectionContext connection)
  {
    return connection.User?.Identity?.Name;
  }
}
