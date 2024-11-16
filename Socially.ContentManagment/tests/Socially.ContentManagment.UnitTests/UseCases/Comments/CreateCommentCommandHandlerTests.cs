using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Moq;
using Socially.ContentManagment.Core.PostAggregate;
using Socially.ContentManagment.Core.PostAggregate.Specifications;
using Socially.ContentManagment.UseCases.Comments.Common.DTOs;
using Socially.ContentManagment.UseCases.Comments.Create;
using Xunit;

namespace Socially.ContentManagment.UnitTests.UseCases.Comments;

public class CreateCommentCommandHandlerTests
{
  [Fact]
  public async Task Handle_ShouldAddCommentSuccessfully_WhenPostExists()
  {
    // Arrange
    var mockRepository = new Mock<IRepository<Post>>();
    var postId = Guid.NewGuid();
    var userId = Guid.NewGuid();

    var existingPost = new Post(postId, userId, "Sample Content", Privacy.Public);
    var createCommentDto = new CreateCommentDto
    {
      PostId = postId,
      Content = "This is a new comment"
    };
    var command = new CreateCommentCommand(createCommentDto, userId);

    mockRepository
        .Setup(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(existingPost);

    var handler = new CreateCommentCommandHandler(mockRepository.Object);

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

    var createCommentDto = new CreateCommentDto
    {
      PostId = postId,
      Content = "This is a new comment"
    };
    var command = new CreateCommentCommand(createCommentDto, userId);

    mockRepository
        .Setup(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Post?)null); // Simulate post not found

    var handler = new CreateCommentCommandHandler(mockRepository.Object);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.False(result.IsSuccess);

    // Log actual errors for debugging
    foreach (var error in result.Errors)
    {
      Console.WriteLine(error);
    }

    // Use a substring match to account for formatting differences
    Assert.Contains("Posts.NotFound", result.Errors.FirstOrDefault());

    mockRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()), Times.Once);
  }


}
