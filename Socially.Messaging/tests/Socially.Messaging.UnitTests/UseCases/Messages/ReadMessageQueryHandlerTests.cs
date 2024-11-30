using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Moq;
using Socially.Messaging.Core.MessageAggregate;
using Socially.Messaging.Core.MessageAggregate.Specifications;
using Socially.Messaging.UseCases.Messages.Common.DTOs;
using Socially.Messaging.UseCases.Messages.Read;
using Xunit;


namespace Socially.Messaging.UnitTests.UseCases.Messages;
public class ReadMessageQueryHandlerTests
{
  [Fact]
  public async Task Handle_ReturnsMessagesMappedToDtos_WhenMessagesExist()
  {
    // Arrange
    var mockRepository = new Mock<IReadRepository<Message>>();
    var handler = new ReadMessageQueryHandler(mockRepository.Object);

    var senderId = Guid.NewGuid();
    var receiverId = Guid.NewGuid();
    var query = new ReadMessageQuery(senderId,receiverId);

    var messages = new List<Message>
        {
            new Message("Hello", senderId, receiverId),
            new Message("Hi", senderId, receiverId)
        };

    mockRepository
        .Setup(repo => repo.ListAsync(It.IsAny<MessageBySenderNReceiver>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(messages);

    // Act
    var result = await handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(ResultStatus.Ok, result.Status);
    Assert.NotNull(result.Value);
    Assert.Equal(2, result.Value.Count);

    Assert.Collection(result.Value,
        dto => Assert.Equal("Hello", dto.Content),
        dto => Assert.Equal("Hi", dto.Content));

    mockRepository.Verify(repo => repo.ListAsync(It.IsAny<MessageBySenderNReceiver>(), It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_ReturnsSuccessWithEmptyList_WhenNoMessagesExist()
  {
    // Arrange
    var mockRepository = new Mock<IReadRepository<Message>>();
    var handler = new ReadMessageQueryHandler(mockRepository.Object);

    var senderId = Guid.NewGuid();
    var receiverId = Guid.NewGuid();
    var query = new ReadMessageQuery (senderId, receiverId);

    mockRepository
        .Setup(repo => repo.ListAsync(It.IsAny<MessageBySenderNReceiver>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new List<Message>());

    // Act
    var result = await handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(ResultStatus.Ok, result.Status);
    Assert.NotNull(result.Value);
    Assert.Empty(result.Value);

    mockRepository.Verify(repo => repo.ListAsync(It.IsAny<MessageBySenderNReceiver>(), It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_ReturnsFailure_WhenRepositoryThrowsException()
  {
    // Arrange
    var mockRepository = new Mock<IReadRepository<Message>>();
    var handler = new ReadMessageQueryHandler(mockRepository.Object);

    var senderId = Guid.NewGuid();
    var receiverId = Guid.NewGuid();
    var query = new ReadMessageQuery (senderId, receiverId);

    mockRepository
        .Setup(repo => repo.ListAsync(It.IsAny<MessageBySenderNReceiver>(), It.IsAny<CancellationToken>()))
        .ThrowsAsync(new Exception("Database error"));

    // Act & Assert
    var exception = await Assert.ThrowsAsync<Exception>(async () =>
        await handler.Handle(query, CancellationToken.None)
    );

    Assert.Equal("Database error", exception.Message);

    mockRepository.Verify(repo => repo.ListAsync(It.IsAny<MessageBySenderNReceiver>(), It.IsAny<CancellationToken>()), Times.Once);
  }
}
