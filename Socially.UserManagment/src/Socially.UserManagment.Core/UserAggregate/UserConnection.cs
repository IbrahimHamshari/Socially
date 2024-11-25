using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Socially.UserManagment.Core.UserAggregate;
public class UserConnection : EntityBase<Guid>
{
  public Guid FollowerId { get; private set; }
  public Guid FollowedId { get; private set; }

  public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

  public UserConnection(Guid followerId, Guid followedId)
  {
    FollowerId = Guard.Against.Default(followerId, nameof(followerId));
    FollowedId = Guard.Against.Default(followedId, nameof(followedId));
  }
}
