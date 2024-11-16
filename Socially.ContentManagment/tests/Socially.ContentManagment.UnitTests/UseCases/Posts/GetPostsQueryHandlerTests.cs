using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Moq;
using Socially.ContentManagment.Core.PostAggregate;
using Socially.ContentManagment.Core.PostAggregate.Specifications;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;
using Socially.ContentManagment.UseCases.Posts.Get;
using Xunit;

namespace Socially.ContentManagment.UnitTests.UseCases.Posts;

public class GetPostsQueryHandlerTests
{
  [Fact]
  public async Task Handle_ShouldReturnPosts_WhenPostsExist()
  {
    // Arrange
    var mockRepository = new Mock<IReadRepository<Post>>();
    var userId = Guid.NewGuid();

    var post1 = new Post(Guid.NewGuid(), userId, "Post 1 Content", Privacy.Public, "https://example.com/media1");
    var post2 = new Post(Guid.NewGuid(), userId, "Post 2 Content", Privacy.Private, "https://example.com/media2");

    var posts = new List<Post> { post1, post2 };
    var query = new GetPostsQuery(userId);

    mockRepository
        .Setup(r => r.ListAsync(It.IsAny<PostsByUserIdSpec>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(posts);

    var handler = new GetPostsQueryHandler(mockRepository.Object);

    // Act
    var result = await handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Value);
    Assert.Equal(posts.Count, result.Value.Length);

    // Validate the properties of the first post
    Assert.Equal(post1.Id, result.Value[0].Id);
    Assert.Equal(post1.Content, result.Value[0].Content);
    Assert.Equal(post1.MediaURL, result.Value[0].MediaURL);
    Assert.Equal(post1.Privacy, result.Value[0].Privacy);
    Assert.Equal(post1.UserID, result.Value[0].UserId);

    // Verify correct repository method was called
    mockRepository.Verify(r => r.ListAsync(It.IsAny<PostsByUserIdSpec>(), It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_ShouldReturnEmptyArray_WhenNoPostsExist()
  {
    // Arrange
    var mockRepository = new Mock<IReadRepository<Post>>();
    var userId = Guid.NewGuid();

    var query = new GetPostsQuery(userId);

    mockRepository
        .Setup(r => r.ListAsync(It.IsAny<PostsByUserIdSpec>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new List<Post>()); // Simulate no posts found

    var handler = new GetPostsQueryHandler(mockRepository.Object);

    // Act
    var result = await handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Value);
    Assert.Empty(result.Value); // Should return an empty array

    // Verify correct repository method was called
    mockRepository.Verify(r => r.ListAsync(It.IsAny<PostsByUserIdSpec>(), It.IsAny<CancellationToken>()), Times.Once);
  }
}
