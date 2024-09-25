using Ardalis.SharedKernel;

namespace Socially.UserManagment.Core.UserAggregate.Events;

public class UserCreatedEvent(User user) : DomainEventBase
{
  public User User { get; init; } = user;
}
