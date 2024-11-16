using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Moq;
using Socially.ContentManagment.Core.PostAggregate;
using Socially.ContentManagment.Core.PostAggregate.Specifications;
using Socially.ContentManagment.UseCases.Likes.Common.DTOs;
using Socially.ContentManagment.UseCases.Likes.Create;
using Xunit;

namespace Socially.ContentManagment.UnitTests.UseCases.Likes;

public class ToggleLikeCommandHandlerTests
{
  [Fact]
  public async Task Handle_ShouldToggleLikeOnPost_WhenCommentIdIsNull()
  {
    // Arrange
    var mockRepository = new Mock<IRepository<Post>>();
    var postId = Guid.NewGuid();
    var userId = Guid.NewGuid();

    var existingPost = new Post(postId, userId, "Sample Content", Privacy.Public);
    var toggleLikeDto = new ToggleLikeDto
    {
      PostId = postId,
      CommentId = null
    };
    var command = new ToggleLikeCommand(toggleLikeDto, userId);

    mockRepository
        .Setup(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(existingPost);

    var handler = new ToggleLikeCommandHandler(mockRepository.Object);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    mockRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_ShouldToggleLikeOnComment_WhenCommentIdIsProvided()
  {
    // Arrange
    var mockRepository = new Mock<IRepository<Post>>();
    var postId = Guid.NewGuid();
    var userId = Guid.NewGuid();

    var existingPost = new Post(postId, userId, "Sample Content", Privacy.Public);
    existingPost.AddComment(userId, "Sample Content");
    var commentId = existingPost.Comments.FirstOrDefault()!.Id;
    var toggleLikeDto = new ToggleLikeDto
    {
      PostId = postId,
      CommentId = commentId
    };
    var command = new ToggleLikeCommand(toggleLikeDto, userId);

    mockRepository
        .Setup(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(existingPost);

    var handler = new ToggleLikeCommandHandler(mockRepository.Object);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    mockRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_ShouldReturnError_WhenPostNotFound()
  {
    // Arrange
    var mockRepository = new Mock<IRepository<Post>>();
    var postId = Guid.NewGuid();
    var userId = Guid.NewGuid();

    var toggleLikeDto = new ToggleLikeDto
    {
      PostId = postId,
      CommentId = null
    };
    var command = new ToggleLikeCommand(toggleLikeDto, userId);

    mockRepository
        .Setup(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Post?)null); // Simulate post not found

    var handler = new ToggleLikeCommandHandler(mockRepository.Object);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.Contains($"Posts.NotFound", result.Errors);

    mockRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()), Times.Once);
  }
}
