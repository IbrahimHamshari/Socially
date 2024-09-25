using Ardalis.SharedKernel;

namespace Socially.UserManagment.Core.UserAggregate.Events;

public class UserDeletedEvent(User user) : DomainEventBase
{
  public User User { get; init; } = user;
}
