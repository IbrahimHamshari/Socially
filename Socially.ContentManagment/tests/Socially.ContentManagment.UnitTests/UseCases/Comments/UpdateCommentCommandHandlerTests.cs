using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Builder;
using Moq;
using Socially.ContentManagment.Core.PostAggregate;
using Socially.ContentManagment.Core.PostAggregate.Specifications;
using Socially.ContentManagment.UseCases.Comments.Common.DTOs;
using Socially.ContentManagment.UseCases.Comments.Update;
using Xunit;

namespace Socially.ContentManagment.UnitTests.UseCases.Comments;

public class UpdateCommentCommandHandlerTests
{
  [Fact]
  public async Task Handle_ShouldUpdateCommentSuccessfully_WhenPostExists()
  {
    // Arrange
    var mockRepository = new Mock<IRepository<Post>>();
    var postId = Guid.NewGuid();
    var userId = Guid.NewGuid();

    var existingPost = new Post(postId, userId, "Sample Content", Privacy.Public);
    existingPost.AddComment(userId, "Original Comment");
    var commentId = existingPost.Comments.FirstOrDefault()!.Id;
    var updateCommentDto = new UpdateCommentDto
    {
      PostId = postId,
      Id = commentId,
      Content = "Updated Comment Content"
    };

    var command = new UpdateCommentCommand(updateCommentDto, userId);

    mockRepository
        .Setup(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(existingPost);

    var handler = new UpdateCommentCommandHandler(mockRepository.Object);
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
    var commentId = Guid.NewGuid();

    var updateCommentDto = new UpdateCommentDto
    {
      PostId = postId,
      Id = commentId,
      Content = "Updated Comment Content"
    };

    var command = new UpdateCommentCommand(updateCommentDto, userId);

    mockRepository
        .Setup(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Post?)null); // Simulate post not found

    var handler = new UpdateCommentCommandHandler(mockRepository.Object);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.Contains("Posts.NotFound", result.Errors.FirstOrDefault());

    mockRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()), Times.Once);
  }
}
