using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SharedKernel;

namespace Socially.ContentManagment.Core.PostAggregate;
public class User : EntityBase<Guid>
{

  public string FirstName { get; private set; } = string.Empty;

  public string LastName { get; private set; } = string.Empty;

  public string? ProfilePictureURL { get; private set; }

  public User(Guid id, string firstName, string lastName, string? profilePictureURL) 
  {
    Id = id;
    FirstName = firstName;
    LastName = lastName;
    ProfilePictureURL = profilePictureURL;
  }


}
