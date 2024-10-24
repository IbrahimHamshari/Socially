using Ardalis.SharedKernel;

namespace Socially.ContentManagment.Core.UserAggregate.Events;

public class AccountRecoveredEvent(User _user) : DomainEventBase
{
  public User User { get; private set; } = _user;
}
