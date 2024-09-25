using Ardalis.SharedKernel;

namespace Socially.UserManagment.Core.UserAggregate.Events;

public class UserVerifiedEvent(User User) : DomainEventBase
{
  public User User { get; init; } = User;
}
