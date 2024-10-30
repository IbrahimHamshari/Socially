﻿using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.PostAggregate;

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

  // Constructor and business logic here
  public Post(Guid id, Guid userId, string content, Privacy privacy, string mediaURL = "")
  {
    Id = id;
    UserID = userId;
    Content = content;
    MediaURL = mediaURL;
    CreatedAt = DateTime.UtcNow;
    Privacy = privacy;
    _comments = new List<Comment>();
    _likes = new List<Like>();
    _shares = new List<Share>();
  }

  public void UpdateContent(string content)
  {
    Content = content;
  }

  public void UpdateMediaURL(string mediaURL) 
  { 
    MediaURL = mediaURL;
  }

  public void UpdatePrivacy(Privacy privacy)
  {
    Privacy = privacy;
  }

  // Business rules for managing Comments, Likes, and Shares should be here
  public void AddComment(Guid userId, string content)
  {
    _comments.Add(new Comment(Guid.NewGuid(), Id, userId, content));
    UpdatedAt = DateTime.UtcNow;
  }

  public void DeleteComment (Guid userId, Guid commentId)
  {
    if(_comments.Any(c => c.Id == commentId))
    {
      _comments.Remove(_comments.Find(c => c.Id == userId)!);
    }
    UpdatedAt = DateTime.UtcNow;
  }
  public void LikePost(Guid userId)
  {
    if (!_likes.Any(l => l.UserID == userId))
    {
      _likes.Add(new Like(userId, Id));
    }
    else
    {
      _likes.Remove(_likes.Find(l => l.UserID == userId)!);
    }
      UpdatedAt = DateTime.UtcNow;
  }

  public void SharePost(Guid userId, string message)
  {
    if (!_shares.Any(s => s.UserID == userId))
    {
      _shares.Add(new Share(Id, userId, message));
    }
    UpdatedAt = DateTime.UtcNow;
  }

  public void RemoveSharedPost(Guid userId)
  {
    
    _shares.Remove(_shares.Find(s => s.UserID == userId)!);
    UpdatedAt = DateTime.UtcNow;
  }
  public void UpdateComment(Guid userId, Guid commentId, string newContent)
  {
    var comment = _comments.FirstOrDefault(c => c.Id == commentId && c.UserID == userId);
    if (comment == null)
    {
      throw new ArgumentException("Comment not found");
    }
    comment.UpdateContent(newContent);
    UpdatedAt = DateTime.UtcNow;
  }

  public void LikeComment(Guid commentId, Guid userId)
  {
    var comment = _comments.FirstOrDefault(c => c.Id == commentId);
    if (comment == null)
    {
      throw new ArgumentException("Comment not found");
    }
      comment.LikeComment(userId);
  }

  public void UpdateShareMessage(Guid userId, string message)
  {
    var share = _shares.FirstOrDefault(s => (s.UserID== userId && s.PostID == Id));
    if (share == null)
    {
      throw new ArgumentException("Share Not Fonud");
    }
    share.UpdateMessage(message);
  }




}
