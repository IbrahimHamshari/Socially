using Microsoft.EntityFrameworkCore;
using Quartz;
using Socially.ContentManagment.Infrastructure.Data;
using Socially.ContentManagment.Infrastructure.Data.Entites;
using Socially.ContentManagment.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Socially.ContentManagment.Infrastructure.BackgroundJobs
{
  [DisallowConcurrentExecution]
  public class ProcessRabbitMqMessagesJob : IJob
  {
    private readonly AppDbContext _dbContext;
    private readonly IRabbitMqConsumerService _rabbitMqConsumerService;

    public ProcessRabbitMqMessagesJob(AppDbContext dbContext, IRabbitMqConsumerService rabbitMqConsumerService)
    {
      _dbContext = dbContext;
      _rabbitMqConsumerService = rabbitMqConsumerService;
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

      try
      {
        // Save the message to the outbox table
        var outboxMessage = new OutboxMessage
        {
          Id = Guid.NewGuid(),
          Type = "UserInformation", // Customize the type if needed
          Content = message,
          OccuredOnUtc = DateTime.UtcNow,
          ProcessedOnUtc = null,
          Error = null
        };

        _dbContext.OutboxMessages.Add(outboxMessage);
        await _dbContext.SaveChangesAsync();

        // Process the message
        await ProcessMessageAsync(outboxMessage);

        // Mark as processed
        outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        // Handle any errors and update the error field
        Console.WriteLine($"Error processing message: {ex.Message}");
      }
    }

    private Task ProcessMessageAsync(OutboxMessage message)
    {
      // Implement your business logic for processing the message here
      Console.WriteLine($"Processing message: {message.Type}, Content: {message.Content}");
      return Task.CompletedTask;
    }
  }
}
