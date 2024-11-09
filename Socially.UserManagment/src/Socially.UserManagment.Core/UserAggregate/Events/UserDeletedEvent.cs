using Ardalis.SharedKernel;
using SharedKernel.Events;

namespace Socially.UserManagment.Core.UserAggregate.Events;

public class UserDeletedEvent(User user) : DomainEventBase, IOutboxEvent
{
  public User User { get; init; } = user;
}
