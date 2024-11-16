using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Moq;
using Socially.ContentManagment.Core.PostAggregate;
using Socially.ContentManagment.Core.PostAggregate.Specifications;
using Socially.ContentManagment.UseCases.Shares.Common.DTOs;
using Socially.ContentManagment.UseCases.Shares.Delete;
using Xunit;

namespace Socially.ContentManagment.UnitTests.UseCases.Shares;

public class DeleteShareCommandHandlerTests
{
  [Fact]
  public async Task Handle_ShouldDeleteSharedPostSuccessfully_WhenPostExists()
  {
    // Arrange
    var mockRepository = new Mock<IRepository<Post>>();
    var postId = Guid.NewGuid();
    var userId = Guid.NewGuid();

    var existingPost = new Post(postId, userId, "Sample Content", Privacy.Public);
    var deleteShareDto = new SharePostDto
    {
      PostId = postId,
      Message = "Removing shared post"
    };
    var command = new DeleteShareCommand(deleteShareDto, userId);

    mockRepository
        .Setup(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(existingPost);

    var handler = new DeleteShareCommandHandler(mockRepository.Object);

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

    var deleteShareDto = new SharePostDto
    {
      PostId = postId,
      Message = "Removing shared post"
    };
    var command = new DeleteShareCommand(deleteShareDto, userId);

    mockRepository
        .Setup(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Post?)null); // Simulate post not found

    var handler = new DeleteShareCommandHandler(mockRepository.Object);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.Contains($"Posts.NotFound", result.Errors);

    mockRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()), Times.Once);
  }
}
