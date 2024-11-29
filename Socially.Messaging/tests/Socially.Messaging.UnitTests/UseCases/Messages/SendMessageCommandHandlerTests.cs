using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Moq;
using Socially.Messaging.Core.MessageAggregate;
using Socially.Messaging.Infrastructure.Interfaces;
using Socially.Messaging.UseCases.Messages.Common.DTOs;
using Socially.Messaging.UseCases.Messages.Send;
using Xunit;

namespace Socially.Messaging.UnitTests.UseCases.Messages;

public class SendMessageCommandHandlerTests
{
  [Fact]
  public async Task Handle_AddsMessageAndNotifiesUser_WhenRequestIsValid()
  {
    // Arrange
    var mockRepository = new Mock<IRepository<Message>>();
    var mockNotificationService = new Mock<INotificationService>();
    var handler = new SendMessageCommandHandler(mockRepository.Object, mockNotificationService.Object);

    var senderId = Guid.NewGuid();
    var receiverId = Guid.NewGuid();
    var messageContent = "Hello, this is a test message.";
    var command = new SendMessageCommand(
        new SendMessageDto
        {
          Content = messageContent,
          ReceiverId = receiverId
        },
        senderId
    );

    var messageId = Guid.NewGuid();
    var message = new Message(messageContent, senderId, receiverId) { Id = messageId };

    // Explicitly mock AddAsync with CancellationToken parameter
    mockRepository
        .Setup(repo => repo.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Message m, CancellationToken token) =>
        {
          m.Id = messageId; // Simulate the repository assigning an ID
          return m;
        });

    // Mock NotifyUserAsync
    mockNotificationService
        .Setup(service => service.NotifyUserAsync(receiverId, It.Is<MessageDto>(dto =>
            dto.Content == messageContent &&
            dto.Id == messageId
        )))
        .Returns(Task.CompletedTask);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(ResultStatus.Ok, result.Status);
    Assert.Equal(messageId, result.Value);

    mockRepository.Verify(repo => repo.AddAsync(It.Is<Message>(m =>
        m.Content == messageContent &&
        m.SenderId == senderId &&
        m.ReceiverId == receiverId
    ), It.IsAny<CancellationToken>()), Times.Once);

    mockNotificationService.Verify(service => service.NotifyUserAsync(receiverId, It.Is<MessageDto>(dto =>
        dto.Content == messageContent &&
        dto.Id == messageId
    )), Times.Once);
  }

  [Fact]
  public async Task Handle_ReturnsFailure_WhenRepositoryThrowsException()
  {
    // Arrange
    var mockRepository = new Mock<IRepository<Message>>();
    var mockNotificationService = new Mock<INotificationService>();
    var handler = new SendMessageCommandHandler(mockRepository.Object, mockNotificationService.Object);

    var senderId = Guid.NewGuid();
    var receiverId = Guid.NewGuid();
    var messageContent = "This should fail.";
    var command = new SendMessageCommand(
        new SendMessageDto
        {
          Content = messageContent,
          ReceiverId = receiverId
        },
        senderId
    );

    // Simulate an exception in AddAsync
    mockRepository
        .Setup(repo => repo.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()))
        .ThrowsAsync(new Exception("Database error"));

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(ResultStatus.Error, result.Status);
    Assert.Contains("Database error", result.Errors.First());

    mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()), Times.Once);
    mockNotificationService.Verify(service => service.NotifyUserAsync(It.IsAny<Guid>(), It.IsAny<MessageDto>()), Times.Never);
  }

  [Fact]
  public async Task Handle_ReturnsFailure_WhenNotificationServiceThrowsException()
  {
    // Arrange
    var mockRepository = new Mock<IRepository<Message>>();
    var mockNotificationService = new Mock<INotificationService>();
    var handler = new SendMessageCommandHandler(mockRepository.Object, mockNotificationService.Object);

    var senderId = Guid.NewGuid();
    var receiverId = Guid.NewGuid();
    var messageContent = "Notification failure test.";
    var command = new SendMessageCommand(
        new SendMessageDto
        {
          Content = messageContent,
          ReceiverId = receiverId
        },
        senderId
    );

    var messageId = Guid.NewGuid();
    var message = new Message(messageContent, senderId, receiverId) { Id = messageId };

    // Mock AddAsync
    mockRepository
        .Setup(repo => repo.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Message m, CancellationToken token) =>
        {
          m.Id = messageId;
          return m;
        });

    // Simulate an exception in NotifyUserAsync
    mockNotificationService
        .Setup(service => service.NotifyUserAsync(receiverId, It.IsAny<MessageDto>()))
        .ThrowsAsync(new Exception("Notification error"));

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(ResultStatus.Error, result.Status);
    Assert.Contains("Notification error", result.Errors.First());

    mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()), Times.Once);
    mockNotificationService.Verify(service => service.NotifyUserAsync(receiverId, It.IsAny<MessageDto>()), Times.Once);
  }
}
