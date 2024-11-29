using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Moq;
using Socially.Messaging.Core.MessageAggregate;
using Socially.Messaging.Core.MessageAggregate.Errors;
using Socially.Messaging.Core.MessageAggregate.Specifications;
using Socially.Messaging.Infrastructure.Interfaces;
using Socially.Messaging.UseCases.Messages.MarkMessageAsRead;
using Xunit;

namespace Socially.Messaging.UnitTests.UseCases.Messages;

public class MarkMessageAsReadCommandHandlerTests
{
  [Fact]
  public async Task Handle_ReturnsSuccess_WhenMessageIsMarkedAsRead()
  {
    // Arrange
    var mockRepository = new Mock<IRepository<Message>>();
    var mockNotificationService = new Mock<INotificationService>();
    var handler = new MarkMessageAsReadCommandHandler(mockRepository.Object, mockNotificationService.Object);

    var messageId = Guid.NewGuid();
    var senderId = Guid.NewGuid();
    var message = new Message("Test content", senderId, Guid.NewGuid()) { Id = messageId };

    var command = new MarkMessageAsReadCommand(messageId);

    // Mock repository to return the message
    mockRepository
        .Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<MessageById>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(message);

    // Mock repository SaveChangesAsync to return an integer
    mockRepository
        .Setup(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(1);

    // Mock notification service
    mockNotificationService
        .Setup(service => service.NotifyStatusUpdateAsync(senderId, messageId, "Read"))
        .Returns(Task.CompletedTask);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(ResultStatus.Ok, result.Status);

    mockRepository.Verify(repo =>
        repo.SingleOrDefaultAsync(It.IsAny<MessageById>(), It.IsAny<CancellationToken>()),
        Times.Once);

    mockRepository.Verify(repo =>
        repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
        Times.Once);

    mockNotificationService.Verify(service =>
        service.NotifyStatusUpdateAsync(senderId, messageId, "Read"),
        Times.Once);
  }

  [Fact]
  public async Task Handle_ReturnsNotFound_WhenMessageDoesNotExist()
  {
    // Arrange
    var mockRepository = new Mock<IRepository<Message>>();
    var mockNotificationService = new Mock<INotificationService>();
    var handler = new MarkMessageAsReadCommandHandler(mockRepository.Object, mockNotificationService.Object);

    var messageId = Guid.NewGuid();
    var command = new MarkMessageAsReadCommand(messageId);

    // Mock repository to return null
    mockRepository
        .Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<MessageById>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Message?)null); // Explicitly cast null to nullable type

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(ResultStatus.NotFound, result.Status);
    Assert.Contains(messageId.ToString(), result.Errors.First());

    mockRepository.Verify(repo =>
        repo.SingleOrDefaultAsync(It.IsAny<MessageById>(), It.IsAny<CancellationToken>()),
        Times.Once);

    mockRepository.Verify(repo =>
        repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
        Times.Never);

    mockNotificationService.Verify(service =>
        service.NotifyStatusUpdateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()),
        Times.Never);
  }


  [Fact]
  public async Task Handle_ThrowsException_WhenSaveChangesFails()
  {
    // Arrange
    var mockRepository = new Mock<IRepository<Message>>();
    var mockNotificationService = new Mock<INotificationService>();
    var handler = new MarkMessageAsReadCommandHandler(mockRepository.Object, mockNotificationService.Object);

    var messageId = Guid.NewGuid();
    var senderId = Guid.NewGuid();
    var message = new Message("Test content", senderId, Guid.NewGuid()) { Id = messageId };

    var command = new MarkMessageAsReadCommand(messageId);

    // Mock repository to return the message
    mockRepository
        .Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<MessageById>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(message);

    // Mock repository SaveChangesAsync to throw exception
    mockRepository
        .Setup(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()))
        .ThrowsAsync(new Exception("Database error"));

    // Act & Assert
    await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));

    mockRepository.Verify(repo =>
        repo.SingleOrDefaultAsync(It.IsAny<MessageById>(), It.IsAny<CancellationToken>()),
        Times.Once);

    mockRepository.Verify(repo =>
        repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
        Times.Once);

    mockNotificationService.Verify(service =>
        service.NotifyStatusUpdateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()),
        Times.Never);
  }
}
