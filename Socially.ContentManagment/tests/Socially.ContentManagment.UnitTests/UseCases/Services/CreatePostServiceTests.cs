using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Moq;
using Socially.ContentManagment.Core.PostAggregate;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;
using Socially.ContentManagment.UseCases.Posts.Services;
using Xunit;

namespace Socially.ContentManagment.UnitTests.UseCases.Services;

public class CreatePostServiceTests
{
  [Fact]
  public async Task CreatePost_ShouldReturnCreatedResult_WhenPostIsAddedSuccessfully()
  {
    // Arrange
    var mockRepository = new Mock<Ardalis.SharedKernel.IRepository<Post>>();

    var userId = Guid.NewGuid();
    var content = "Test content";
    var privacy = 1;
    var mediaUrl = "https://example.com/media";
    var postId = Guid.NewGuid();

    var newPost = new Post(postId, userId, content, Privacy.Public, mediaUrl);

    var postDto = new PostDto
    {
      Id = postId,
      Content = content,
      Privacy = (int)Privacy.Public,
      MediaURL = mediaUrl,
      UserId = userId
    };

    // Mock AddAsync with explicit CancellationToken
    mockRepository
        .Setup(r => r.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(newPost);

    var service = new CreatePostService(mockRepository.Object);

    // Act
    var result = await service.CreatePost(userId, content, privacy, mediaUrl);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal(content, result.Value.Content);
    Assert.Equal(userId, result.Value.UserId);
    Assert.Equal(mediaUrl, result.Value.MediaURL);
    Assert.Equal((int)Privacy.Public, result.Value.Privacy);

    // Verify AddAsync was called once
    mockRepository.Verify(r => r.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Once);
  }
}
