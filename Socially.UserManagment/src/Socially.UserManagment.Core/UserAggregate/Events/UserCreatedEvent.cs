using Ardalis.SharedKernel;
using SharedKernel.Events;
using Socially.ContentManagment.Core.Interfaces;

namespace Socially.ContentManagment.Core.UserAggregate.Events;

public class UserCreatedEvent(User user) : DomainEventBase, IOutboxEvent
{
  public User User { get; init; } = user;
}
