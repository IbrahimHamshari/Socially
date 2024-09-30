using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.SharedKernel;

namespace Socially.ContentManagment.Core.PostAggregate;

public class Post : EntityBase<Guid>, IAggregateRoot
{
  public Guid UserID { get; private set; }
  public string Content { get; private set; }
  public string MediaURL { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }
  public Privacy Privacy { get; private set; }

  private List<Comment> _comments;
  private List<Like> _likes;
  private List<Share> _shares;

  public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
  public IReadOnlyCollection<Like> Likes => _likes.AsReadOnly();
  public IReadOnlyCollection<Share> Shares => _shares.AsReadOnly();

  // Constructor for creating a post
  public Post(Guid userId, string content, string mediaURL, Privacy privacy)
  {
    Id = Guid.NewGuid();
    UserID = userId;
    Content = content;
    MediaURL = mediaURL;
    CreatedAt = DateTime.UtcNow;
    Privacy = privacy;

    _comments = new List<Comment>();
    _likes = new List<Like>();
    _shares = new List<Share>();
  }

  // Add a comment
  public void AddComment(Guid userId, string content)
  {
    _comments.Add(new Comment(Guid.NewGuid(), Id, userId, content));
    UpdatedAt = DateTime.UtcNow; // Update timestamp when a comment is added
  }

  // Like a post
  public void LikePost(Guid userId)
  {
    if (!_likes.Any(l => l.UserID == userId))
    {
      _likes.Add(new Like(userId, Id));
      UpdatedAt = DateTime.UtcNow; // Update timestamp when the post is liked
    }
  }

  // Share a post
  public void SharePost(Guid userId, string message)
  {
    _shares.Add(new Share(Id, userId, message));
    UpdatedAt = DateTime.UtcNow; // Update timestamp when the post is shared
  }

  // Update content
  public void UpdateContent(string newContent)
  {
    Content = newContent;
    UpdatedAt = DateTime.UtcNow; // Update timestamp when content is modified
  }

  // Update media URL
  public void UpdateMediaURL(string newMediaURL)
  {
    MediaURL = newMediaURL;
    UpdatedAt = DateTime.UtcNow; // Update timestamp when media URL is modified
  }

  // Update privacy
  public void UpdatePrivacy(Privacy newPrivacy)
  {
    Privacy = newPrivacy;
    UpdatedAt = DateTime.UtcNow; // Update timestamp when privacy setting is modified
  }

  // Update post's user
  public void UpdateUserID(Guid newUserId)
  {
    UserID = newUserId;
    UpdatedAt = DateTime.UtcNow; // Update timestamp when user is modified
  }
}
