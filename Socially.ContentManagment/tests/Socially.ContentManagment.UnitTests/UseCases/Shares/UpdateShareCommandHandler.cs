using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Moq;
using Socially.ContentManagment.Core.PostAggregate;
using Socially.ContentManagment.Core.PostAggregate.Specifications;
using Socially.ContentManagment.UseCases.Shares.Update;
using Socially.ContentManagment.UseCases.Shares.Common.DTOs;
using Xunit;
using Ardalis.SharedKernel;

namespace Socially.ContentManagment.UnitTests.UseCases.Shares;

public class UpdateShareCommandHandlerTests
{
  [Fact]
  public async Task Handle_ShouldUpdateShareMessageSuccessfully_WhenPostExists()
  {
    // Arrange
    var mockRepository = new Mock<IRepository<Post>>();
    var postId = Guid.NewGuid();
    var userId = Guid.NewGuid();

    var existingPost = new Post(postId, userId, "Sample Content", Privacy.Public);
    existingPost.SharePost(userId, "Sample Share Content");
    var updateShareDto = new SharePostDto
    {
      PostId = postId,
      Message = "Updated Share Message"
    };
    var command = new UpdateShareCommand(updateShareDto, userId);

    mockRepository
        .Setup(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(existingPost);

    var handler = new UpdateShareCommandHandler(mockRepository.Object);

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

    var updateShareDto = new SharePostDto
    {
      PostId = postId,
      Message = "Updated Share Message"
    };
    var command = new UpdateShareCommand(updateShareDto, userId);

    mockRepository
        .Setup(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Post?)null); // Simulate post not found

    var handler = new UpdateShareCommandHandler(mockRepository.Object);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.Contains($"Posts.NotFound", result.Errors);

    mockRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<PostByIdSpec>(), It.IsAny<CancellationToken>()), Times.Once);
  }
}
