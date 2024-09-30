using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SharedKernel;

namespace Socially.SocialGroup.Core.SyncedAggregate;
public class User : EntityBase<Guid>,IAggregateRoot
{
  public string FirstName { get; private set; } = string.Empty;
  public string LastName { get; private set;} = string.Empty;
  public string? ProfilePictureURL { get; private set; }
  public string? CoverPhotoURL { get; private set;}
  public string? Bio { get; private set; }
}
