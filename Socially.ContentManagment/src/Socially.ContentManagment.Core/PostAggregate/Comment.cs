using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SharedKernel;

namespace Socially.ContentManagment.Core.PostAggregate;
public class Comment : EntityBase<Guid>
{
  public Guid CommentID { get; private set; }
  public Guid PostID { get; private set; }
  public Guid UserID { get; private set; }
  public string Content { get; private set; }
  public DateTime CreatedAt { get; private set; }

  private List<Like> _likes;
  public IReadOnlyCollection<Like> Likes => _likes.AsReadOnly();

  public Comment(Guid id, Guid postId, Guid userId, string content)
  {
    Id = Guid.NewGuid();
    CommentID = Guid.NewGuid();
    PostID = postId;
    UserID = userId;
    Content = content;
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
