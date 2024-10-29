using Ardalis.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using Socially.ContentManagment.Infrastructure.Data;
using Socially.ContentManagment.Infrastructure.Messaging;
using Socially.UserManagment.Infrastructure.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.ContentManagment.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
  private readonly AppDbContext _dbContext;
  private readonly IRabbitMqService _rabbitMqService;
  public ProcessOutboxMessagesJob(AppDbContext dbContext, IRabbitMqService rabbitMqService)
  {
    _dbContext = dbContext;
    _rabbitMqService = rabbitMqService;
  }

  public async Task Execute(IJobExecutionContext context)
  {
    List<OutboxMessage> messages = await _dbContext
      .Set<OutboxMessage>()
      .Where(m => m.ProcessedOnUtc == null)
      .Take(20)
      .ToListAsync(context.CancellationToken);

    foreach (OutboxMessage outboxMessage in messages)
    {
      try
      {
        var messageContent = JsonConvert.SerializeObject(new
        {
          Id = outboxMessage.Id,
          Type = outboxMessage.Type,
          Content = outboxMessage.Content,
          OccuredOnUtc = outboxMessage.OccuredOnUtc
        });

        _rabbitMqService.PublishMessage(messageContent);

        outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        outboxMessage.Error = null; 
      }
      catch (Exception ex)
      {
        outboxMessage.Error = ex.Message;
      }
    }

    await _dbContext.SaveChangesAsync(context.CancellationToken);



  }
}
