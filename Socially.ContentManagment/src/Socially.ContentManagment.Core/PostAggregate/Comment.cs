using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.PostAggregate.Guards;

namespace Socially.ContentManagment.Core.PostAggregate;
public class Comment : EntityBase<Guid>
{
  public Guid PostID { get; private set; }
  public Guid UserID { get; private set; }
  public string Content { get; private set; }
  public DateTime CreatedAt { get; private set; }

  private List<Like> _likes;
  public IReadOnlyCollection<Like> Likes => _likes.AsReadOnly();

  public Comment(Guid id, Guid postId, Guid userId, string content)
  {
    Id = Guard.Against.Default(id,nameof(id));
    PostID = Guard.Against.Default(postId,nameof(postId));
    UserID = Guard.Against.Default(userId,nameof(userId));
    Content = Guard.Against.InvalidContentFormat(content, nameof(content));
    CreatedAt = DateTime.UtcNow;
    _likes = new List<Like>();
  }

  // Add a like to this comment
  public void LikeComment(Guid userId)
  {
    if (!_likes.Any(l => l.UserID == userId))
    {
      _likes.Add(new Like(userId, null, this.CommentID));
    }
  }
}
