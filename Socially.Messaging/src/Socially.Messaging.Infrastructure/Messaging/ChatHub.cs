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

}
