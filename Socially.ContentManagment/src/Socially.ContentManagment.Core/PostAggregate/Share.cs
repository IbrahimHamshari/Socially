using System;
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
  public DateTime UpdatedAt { get; private set; }

  // Constructor for creating a share
  public Share(Guid postId, Guid userId, string message)
  {
    PostID = Guard.Against.Default(postId, nameof(postId));
    UserID = Guard.Against.Default(userId, nameof(userId));
    Message = Guard.Against.InvalidContentFormat(message, nameof(message));
    SharedAt = DateTime.UtcNow;
    UpdatedAt = DateTime.UtcNow;
  }

  // Update the Message
  public void UpdateMessage(string newMessage)
  {
    Message = Guard.Against.InvalidContentFormat(newMessage, nameof(newMessage));
    UpdatedAt = DateTime.UtcNow; // Update timestamp when Message is changed
  }
}
