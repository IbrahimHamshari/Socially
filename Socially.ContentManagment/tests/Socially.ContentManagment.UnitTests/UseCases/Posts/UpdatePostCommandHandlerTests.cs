using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Moq;
using Microsoft.AspNetCore.Http;
using Socially.ContentManagment.Core.PostAggregate;
using Socially.ContentManagment.Core.PostAggregate.Specifications;
using Socially.ContentManagment.UseCases.Interfaces;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;
using Socially.ContentManagment.UseCases.Posts.Update;
using Socially.ContentManagment.UseCases.Posts.Utils;
using Xunit;
using Ardalis.SharedKernel;

namespace Socially.ContentManagment.UnitTests.UseCases.Posts;

public class UpdatePostCommandHandlerTests
{
  [Fact]
  public async Task Handle_ShouldUpdatePostSuccessfully_WhenAllFieldsProvided()
  {
    // Arrange
    var mockRepository = new Mock<IRepository<Post>>();
    var mockMediaService = new Mock<IMediaUploadService>();

    var postId = Guid.NewGuid();
    var userId = Guid.NewGuid();

    var existingPost = new Post(postId, userId, "Old Content", Privacy.Public, "https://example.com/old-media");
    var updatedMediaUrl = "https://example.com/new-media";
    var updatePostDto = new UpdatePostDto
    {
      Id = postId,
      Content = "New Content",
      Media = MockMediaFile(),
      Privacy = (int)Privacy.Private
    };

    var command = new UpdatePostCommand(updatePostDto, userId);

    mockRepository
        .Setup(r => r.GetByIdAsync(It.IsAny<PostByIdAndUserId>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(existingPost);

    mockMediaService
        .Setup(m => m.UploadMediaAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
        .ReturnsAsync(updatedMediaUrl);

    mockRepository
        .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(1); 

    var handler = new UpdatePostCommandHandler(mockRepository.Object, mockMediaService.Object);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Value);
    Assert.Equal(postId, result.Value.Id);
    Assert.Equal("New Content", result.Value.Content);
    Assert.Equal(updatedMediaUrl, result.Value.MediaURL);
    Assert.Equal((int)Privacy.Private, result.Value.Privacy);

    // Verify repository interactions
    mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<PostByIdAndUserId>(), It.IsAny<CancellationToken>()), Times.Once);
    mockMediaService.Verify(m => m.UploadMediaAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Once);
    mockRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_ShouldReturnError_WhenPostNotFound()
  {
    // Arrange
    var mockRepository = new Mock<IRepository<Post>>();
    var mockMediaService = new Mock<IMediaUploadService>();

    var postId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var updatePostDto = new UpdatePostDto
    {
      Id = postId,
      Content = "New Content"
    };

    var command = new UpdatePostCommand(updatePostDto, userId);

    mockRepository
        .Setup(r => r.GetByIdAsync(It.IsAny<PostByIdAndUserId>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Post?)null); // Simulate not found

    var handler = new UpdatePostCommandHandler(mockRepository.Object, mockMediaService.Object);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.Contains($"Posts.NotFound", result.Errors);

    // Verify repository interaction
    mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<PostByIdAndUserId>(), It.IsAny<CancellationToken>()), Times.Once);
    mockRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
  }

  private IFormFile MockMediaFile()
  {
    var fileName = "test.jpg";
    var content = "Fake image content";
    var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
    return new FormFile(stream, 0, stream.Length, "Media", fileName)
    {
      Headers = new HeaderDictionary(),
      ContentType = "image/jpeg"
    };
  }
}
