using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using Socially.UserManagement.Core.UserAggregate;

namespace Socially.UserManagment.Core.UserAggregate.Events;
public class UserChangedPasswordEvent(User user) : DomainEventBase
{
  public User User { get; private set; } = user;
}


