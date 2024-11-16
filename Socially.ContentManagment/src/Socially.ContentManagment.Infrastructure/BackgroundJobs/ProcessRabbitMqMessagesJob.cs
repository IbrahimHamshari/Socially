using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using Socially.ContentManagment.Core.PostAggregate;
using Socially.ContentManagment.Infrastructure.Data;
using Socially.ContentManagment.Infrastructure.Data.Entites;
using Socially.ContentManagment.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Socially.ContentManagment.Infrastructure.BackgroundJobs;

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
      var inboxMessage = new InboxMessage
      {
        Id = Guid.NewGuid(),
        Type = "CreatedUserEvent", // Customize the type if needed
        Content = message,
        OccuredOnUtc = DateTime.UtcNow,
        ProcessedOnUtc = null,
        Error = null
      };

      _dbContext.InboxMessages.Add(inboxMessage);
      await _dbContext.SaveChangesAsync();

      // Process the message
      await ProcessMessageAsync(inboxMessage);

      // Mark as processed
      inboxMessage.ProcessedOnUtc = DateTime.UtcNow;
      await _dbContext.SaveChangesAsync();
    }
    catch (Exception ex)
    {
      // Handle any errors and update the error field
      Console.WriteLine($"Error processing message: {ex.Message}");
    }
  }

  private async Task ProcessMessageAsync(InboxMessage message)
  {
    var user = JsonConvert.DeserializeObject<User>(message.Content);
    if(user == null)
    {
      throw new ValidationException("The Content of the message is not suitable for JSON.");
    }
    _dbContext.Users.Add(user);
    await _dbContext.SaveChangesAsync();
    return;
  }
}
