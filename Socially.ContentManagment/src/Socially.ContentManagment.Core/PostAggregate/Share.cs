using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.PostAggregate.Guards;

namespace Socially.ContentManagment.Core.PostAggregate;
public class Share : EntityBase<int>
{
  public Guid PostID { get; private set; }
  public Guid UserID { get; private set; }
  public string Message { get; private set; }
  public DateTime SharedAt { get; private set; }

  public Share(Guid postId, Guid userId, string message)
  {
    PostID = Guard.Against.Default(postId);
    UserID = Guard.Against.Default(userId);
    Message = Guard.Against.InvalidContentFormat(message, nameof(message);
    SharedAt = DateTime.UtcNow;
  }

}
