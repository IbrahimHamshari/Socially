using System;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Socially.ContentManagment.Core.PostAggregate;

public class Like : EntityBase<int>
{
  public Guid UserID { get; private set; }
  public Guid? PostID { get; private set; }   // Nullable if the like is for a comment
  public Guid? CommentID { get; private set; }  // Nullable if the like is for a post
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }

  // Constructor for creating a like
  public Like(Guid userId, Guid? postId = null, Guid? commentId = null)
  {
    UserID = Guard.Against.Default(userId);
    PostID = postId;
    CommentID = commentId;
    CreatedAt = DateTime.UtcNow;
    UpdatedAt = DateTime.UtcNow;

    // Ensure that either PostID or CommentID is provided
    if (PostID == null && CommentID == null || (PostID != null && CommentID != null))
    {
      throw new ArgumentException("Either PostID or CommentID must be provided");
    }
  }

  // Parameterless constructor for ORM use
  private Like() { }

  // Update the user who liked the post/comment
  public void UpdateUserID(Guid newUserId)
  {
    UserID = Guard.Against.Default(newUserId);
    UpdatedAt = DateTime.UtcNow; // Update the timestamp when user is changed
  }

  // Update the associated post ID
  public void UpdateContentId(Guid newId)
  {
    if (PostID == null)
      PostID = newId;
    else
      CommentID = newId;
    UpdatedAt = DateTime.UtcNow; // Update the timestamp when post is changed
  }

}
