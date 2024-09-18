using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SharedKernel;
using Socially.UserManagement.Core.UserAggregate;

namespace Socially.UserManagment.Core.UserAggregate.Events;
public class UserChangedPasswordEvent(User user) : DomainEventBase
{
  public User User { get; init; } = user;
}
