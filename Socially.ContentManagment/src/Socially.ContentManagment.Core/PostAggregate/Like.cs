using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SharedKernel;

namespace Socially.ContentManagment.Core.PostAggregate;
public class Like : EntityBase<int>
{
  public Guid LikeID { get; private set; }
  public Guid UserID { get; private set; }
  public Guid? PostID { get; private set; }   // Nullable if the like is for a comment
  public Guid? CommentID { get; private set; }  // Nullable if the like is for a post
  public DateTime CreatedAt { get; private set; }

  public Like(Guid userId, Guid? postId, Guid? commentId)
  {
    LikeID = Guid.NewGuid();
    UserID = userId;
    PostID = postId;
    CommentID = commentId;
    CreatedAt = DateTime.UtcNow;

    if (PostID == null && CommentID == null)
    {
      throw new ArgumentException("Either PostID or CommentID must be provided");
    }
  }

}
