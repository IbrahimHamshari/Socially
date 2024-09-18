using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SharedKernel;

namespace Socially.UserManagment.Core.RefreshTokenAggregate;
public class RefreshToken(Guid userId, string token, DateTimeOffset expiration, Guid? parentTokenId, string family) : EntityBase<Guid>, IAggregateRoot
{
  public Guid UserId { get; private set; } = userId;
  public  string Token { get; private set; } = token;
  public DateTimeOffset Expiration { get; private set; } = expiration;
  public bool IsRevoked { get; private set; }

  public string Family { get; private set; } = family;

  public Guid? ParentTokenId { get; private set; } = parentTokenId;

  public DateTimeOffset? RevokedAt { get; private set; }

  public void Revoke()
  {
    IsRevoked = true;
    RevokedAt = DateTimeOffset.UtcNow;
  }

  public bool IsExpired => DateTimeOffset.UtcNow >= Expiration;



}

