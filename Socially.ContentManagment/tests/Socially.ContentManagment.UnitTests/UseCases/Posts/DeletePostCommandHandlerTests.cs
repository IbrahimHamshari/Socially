using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Moq;
using Socially.ContentManagment.UseCases.Interfaces;
using Socially.ContentManagment.UseCases.Posts.Delete;
using Xunit;

namespace Socially.ContentManagment.UnitTests.UseCases.Posts;

public class DeletePostCommandHandlerTests
{
  [Fact]
  public async Task Handle_ShouldReturnSuccess_WhenPostIsDeletedSuccessfully()
  {
    // Arrange
    var mockDeletePostService = new Mock<IDeletePostService>();

    var postId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var command = new DeletePostCommand(postId, userId);

    // Mock the service to return a successful result
    mockDeletePostService
        .Setup(s => s.DeletePost(postId, userId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(Result.Success());

    var handler = new DeletePostCommandHandler(mockDeletePostService.Object);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Empty(result.Errors);

    // Verify that the service was called
    mockDeletePostService.Verify(s => s.DeletePost(postId, userId, It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_ShouldReturnError_WhenPostNotFound()
  {
    // Arrange
    var mockDeletePostService = new Mock<IDeletePostService>();

    var postId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var command = new DeletePostCommand(postId, userId);

    // Mock the service to return an error result
    mockDeletePostService
        .Setup(s => s.DeletePost(postId, userId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(Result.Error(new ErrorList(new[] { $"Post with ID {postId} not found." })));

    var handler = new DeletePostCommandHandler(mockDeletePostService.Object);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.Contains($"Post with ID {postId} not found.", result.Errors);

    // Verify that the service was called
    mockDeletePostService.Verify(s => s.DeletePost(postId, userId, It.IsAny<CancellationToken>()), Times.Once);
  }
}
