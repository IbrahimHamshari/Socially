using Ardalis.SharedKernel;

namespace Socially.ContentManagment.Core.UserAggregate.Events;

public class UserChangedPasswordEvent(User user) : DomainEventBase
{
  public User User { get; private set; } = user;
}
