using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SharedKernel;

namespace Socially.Messaging.Core.MessageAggregate;
public class User : EntityBase<Guid>
{
  public string FirstName { get;private set; }
  public string LastName { get;private set; }

  public User(string firstName, string lastName)
  {
    FirstName = firstName;
    LastName = lastName;
  }
}
