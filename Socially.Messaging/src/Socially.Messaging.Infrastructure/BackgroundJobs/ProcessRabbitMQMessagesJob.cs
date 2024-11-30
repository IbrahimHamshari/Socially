using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using SharedKernel.Messages;
using Socially.Messaging.Core.MessageAggregate;
using Socially.Messaging.Infrastructure.Data;
using Socially.Messaging.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Socially.Messaging.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessRabbitMQMessagesJob : IJob
{
  private readonly AppDbContext _dbContext;
  private readonly IRabbitMqConsumerService _rabbitMqConsumerService;
  private readonly ILogger<ProcessRabbitMQMessagesJob> _logger;

  public ProcessRabbitMQMessagesJob(AppDbContext dbContext, IRabbitMqConsumerService rabbitMqConsumerService, ILogger<ProcessRabbitMQMessagesJob> logger)
  {
    _dbContext = dbContext;
    _rabbitMqConsumerService = rabbitMqConsumerService;
    _logger = logger;
  }

  public async Task Execute(IJobExecutionContext context)
  {
    // Consume a message from RabbitMQ
    var message = await _rabbitMqConsumerService.ConsumeMessageAsync();
    if (string.IsNullOrEmpty(message))
    {

      Console.WriteLine("No message received.");
      return;
    }
    var InboxMessage = JsonConvert.DeserializeObject<OutboxMessage>(message);

    if (InboxMessage == null)
    {
      _logger.LogError("This rabbitmq Message Is not parsable ${message}", message);
      return;
    }

    try
    {
      // Save the message to the inbox table
      var inboxMessage = new InboxMessage
      {
        Id = Guid.NewGuid(),
        Type = InboxMessage.Type,
        Content = InboxMessage.Content,
        OccuredOnUtc = DateTime.UtcNow,
        ProcessedOnUtc = null,
        Error = null
      };

      _dbContext.InboxMessages.Add(inboxMessage);
      await _dbContext.SaveChangesAsync();

    }
    catch (Exception ex)
    {
      // Handle any errors and update the error field
      Console.WriteLine($"Error Creating Inbox Message: {ex.Message}");
    }
  }

}
