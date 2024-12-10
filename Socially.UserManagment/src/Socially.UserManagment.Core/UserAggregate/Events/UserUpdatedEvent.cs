using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SharedKernel;
using SharedKernel.Events;

namespace Socially.UserManagment.Core.UserAggregate.Events;
public class UserUpdatedEvent(User user): DomainEventBase, IOutboxEvent
{
  public User User { get; set; } = user;
}
