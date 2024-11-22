using System;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.PostAggregate.Guards;

namespace Socially.ContentManagment.Core.PostAggregate;

public class Share : EntityBase<int>
{
  public Guid PostId { get; private set; }
  public Guid UserId { get; private set; }
  public string Message { get; private set; }
  public DateTime SharedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }

  // Constructor for creating a share
  public Share(Guid postId, Guid userId, string message)
  {
    PostId = Guard.Against.Default(postId, nameof(postId));
    UserId = Guard.Against.Default(userId, nameof(userId));
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
