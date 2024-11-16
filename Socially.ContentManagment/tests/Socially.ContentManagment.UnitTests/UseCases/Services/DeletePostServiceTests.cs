using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Moq;
using Microsoft.Extensions.Logging;
using Socially.ContentManagment.Core.PostAggregate;
using Socially.ContentManagment.Core.PostAggregate.Specifications;
using Socially.ContentManagment.UseCases.Services;
using Xunit;
using Ardalis.SharedKernel;
using Ardalis.Specification;
using NSubstitute;

namespace Socially.ContentManagment.UnitTests.UseCases.Services;

public class DeletePostServiceTests
{
  [Fact]
  public async Task DeletePost_ShouldReturnSuccess_WhenPostExists()
  {
    // Arrange
    var postId = Guid.NewGuid();
    var userId = Guid.NewGuid();

    var mockRepository = new Mock<IRepository<Post>>();
    var mockLogger = new Mock<ILogger<DeletePostService>>();

    // Simulate finding the post
    var existingPost = new Post(postId, userId, "Sample Content", Privacy.Public);
    mockRepository
        .Setup(r => r.SingleOrDefaultAsync(It.IsAny<ISingleResultSpecification<Post>>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(existingPost);

    // Simulate successful deletion
    mockRepository
        .Setup(r => r.DeleteAsync(existingPost, It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);

    var service = new DeletePostService(mockRepository.Object, mockLogger.Object);

    // Act
    var result = await service.DeletePost(postId, userId, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Empty(result.Errors);

    // Verify the correct interactions
    mockRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<ISingleResultSpecification<Post>>(), It.IsAny<CancellationToken>()), Times.Once);
    mockRepository.Verify(r => r.DeleteAsync(existingPost, It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task DeletePost_ShouldReturnError_WhenPostDoesNotExist()
  {
    // Arrange
    var postId = Guid.NewGuid();
    var userId = Guid.NewGuid();

    var mockRepository = new Mock<IRepository<Post>>();
    var mockLogger = new Mock<ILogger<DeletePostService>>();

    // Simulate not finding the post
    mockRepository
        .Setup(r => r.SingleOrDefaultAsync(It.IsAny<ISingleResultSpecification<Post>>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Post?)null); // Explicitly use nullable type

    var service = new DeletePostService(mockRepository.Object, mockLogger.Object);

    // Act
    var result = await service.DeletePost(postId, userId, CancellationToken.None);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.Contains($"Posts.NotFound", result.Errors);

    // Verify the correct interactions
    mockRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<ISingleResultSpecification<Post>>(), It.IsAny<CancellationToken>()), Times.Once);
    mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Never);
  }
}
