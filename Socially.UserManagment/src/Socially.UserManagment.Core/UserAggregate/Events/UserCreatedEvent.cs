using Ardalis.SharedKernel;
using SharedKernel.Events;
using Socially.UserManagment.Core.Interfaces;

namespace Socially.UserManagment.Core.UserAggregate.Events;

public class UserCreatedEvent(User user) : DomainEventBase, IOutboxEvent
{
  public User User { get; init; } = user;
}
