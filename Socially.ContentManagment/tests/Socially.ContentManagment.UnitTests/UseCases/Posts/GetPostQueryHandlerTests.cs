using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Ardalis.Specification;
using Moq;
using Socially.ContentManagment.Core.PostAggregate;
using Socially.ContentManagment.Core.PostAggregate.Specifications;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;
using Socially.ContentManagment.UseCases.Posts.Get;
using Xunit;

namespace Socially.ContentManagment.UnitTests.UseCases.Posts;

public class GetPostQueryHandlerTests
{
  [Fact]
  public async Task Handle_ShouldReturnSuccess_WhenPostIsFound()
  {
    // Arrange
    var mockRepository = new Mock<IReadRepository<Post>>();
    var postId = Guid.NewGuid();
    var userId = Guid.NewGuid();

    var existingPost = new Post(postId, userId, "Sample Content", Privacy.Public, "https://example.com/media");
    var query = new GetPostQuery(postId); // Pass the required 'id' parameter

    mockRepository
        .Setup(r => r.SingleOrDefaultAsync(It.IsAny<ISingleResultSpecification<Post>>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(existingPost);

    var handler = new GetPostQueryHandler(mockRepository.Object);

    // Act
    var result = await handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Value);
    Assert.Equal(existingPost.Id, result.Value.Id);
    Assert.Equal(existingPost.Content, result.Value.Content);
    Assert.Equal(existingPost.MediaURL, result.Value.MediaURL);
    Assert.Equal(existingPost.Privacy, result.Value.Privacy);
    Assert.Equal(existingPost.UserId, result.Value.UserId);

    // Verify correct repository method was called
    mockRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<ISingleResultSpecification<Post>>(), It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_ShouldReturnError_WhenPostIsNotFound()
  {
    // Arrange
    var mockRepository = new Mock<IReadRepository<Post>>();
    var postId = Guid.NewGuid();
    var query = new GetPostQuery(postId); // Pass the required 'id' parameter

    mockRepository
        .Setup(r => r.SingleOrDefaultAsync(It.IsAny<ISingleResultSpecification<Post>>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Post?)null); // Simulate not found

    var handler = new GetPostQueryHandler(mockRepository.Object);

    // Act
    var result = await handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.Contains($"Posts.NotFound", result.Errors);

    // Verify correct repository method was called
    mockRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<ISingleResultSpecification<Post>>(), It.IsAny<CancellationToken>()), Times.Once);
  }
}
