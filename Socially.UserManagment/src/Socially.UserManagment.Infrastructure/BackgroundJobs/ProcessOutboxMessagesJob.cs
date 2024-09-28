using Ardalis.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using Socially.UserManagment.Infrastructure.Data;
using Socially.UserManagment.Infrastructure.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.UserManagment.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
  private readonly AppDbContext _dbContext;
  private readonly IPublisher _publisher;

  public ProcessOutboxMessagesJob(IMediator publisher, AppDbContext dbContext)
  {
    _publisher = publisher;
    _dbContext = dbContext;
  }

  public async Task Execute(IJobExecutionContext context)
  {
    Console.WriteLine("xxxxxxxxx");
    List<OutboxMessage> messages = await _dbContext
      .Set<OutboxMessage>()
      .Where(m => m.ProcessedOnUtc == null)
      .Take(20)
      .ToListAsync(context.CancellationToken);

    foreach (OutboxMessage outboxMessage in messages)
    {
      outboxMessage.ProcessedOnUtc = DateTime.UtcNow;


    }


    await _dbContext.SaveChangesAsync();



  }
}
