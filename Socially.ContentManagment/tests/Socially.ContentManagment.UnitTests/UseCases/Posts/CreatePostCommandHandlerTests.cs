using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Moq;
using Socially.ContentManagment.UseCases.Interfaces;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;
using Socially.ContentManagment.UseCases.Posts.Create;
using Socially.ContentManagment.UseCases.Posts.Services;
using Xunit;

namespace Socially.ContentManagment.UnitTests.UseCases.Posts;

public class CreatePostCommandHandlerTests
{
  [Fact]
  public async Task Handle_ShouldReturnCreatedResult_WhenPostIsCreatedSuccessfully()
  {
    // Arrange
    var mockMediaService = new Mock<IMediaUploadService>();
    var mockCreatePostService = new Mock<ICreatePostService>();

    var mediaUrl = "https://example.com/media";
    var postId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var postDto = new PostDto
    {
      Id = postId,
      Content = "Test content",
      Privacy = 1,
      MediaURL = mediaUrl,
      UserId = userId
    };

    // Simulate an IFormFile
    var fileName = "test.jpg";
    var fileContent = "Fake image content";
    var fileStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
    var formFile = new FormFile(fileStream, 0, fileStream.Length, "Media", fileName)
    {
      Headers = new HeaderDictionary(),
      ContentType = "image/jpeg"
    };

    var createPostDto = new CreatePostDto
    {
      Content = "Test content",
      Privacy = 1,
      Media = formFile
    };

    var createPostCommand = new CreatePostCommand(createPostDto, userId);

    // Mock UploadMediaAsync explicitly handling optional parameters
    mockMediaService
        .Setup(m => m.UploadMediaAsync(formFile, It.IsAny<string>()))
        .ReturnsAsync(mediaUrl);

    mockCreatePostService
        .Setup(s => s.CreatePost(userId, createPostDto.Content, createPostDto.Privacy, mediaUrl))
        .ReturnsAsync(Result.Created(postDto));

    var handler = new CreatePostCommandHandler(mockMediaService.Object, mockCreatePostService.Object);

    // Act
    var result = await handler.Handle(createPostCommand, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal(postDto, result.Value);

    // Verify media service was called with the correct file and optional parameter
    mockMediaService.Verify(m => m.UploadMediaAsync(formFile, It.IsAny<string>()), Times.Once);
  }
}
