﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Quartz;
using SharedKernel.Messages;
using Socially.ContentManagment.Infrastructure.Data;
using Socially.ContentManagment.Core.PostAggregate;
using FluentValidation;

namespace Socially.ContentManagment.Infrastructure.BackgroundJobs;


[DisallowConcurrentExecution]
public class ProcessInboxMessagesJob : IJob
{
  private readonly AppDbContext _dbContext;

  public ProcessInboxMessagesJob(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }
  public async Task Execute(IJobExecutionContext context)
  {
    var inboxMessages = await _dbContext.Set<InboxMessage>().Where(m => m.ProcessedOnUtc == null).Take(20).ToListAsync(context.CancellationToken);

    foreach(var inboxMessage in inboxMessages)
    {
      try
      {
        await ProcessMessageAsync(inboxMessage);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error processing Inbox Message: {ex.Message}");

      }
    }

  }

  private async Task ProcessMessageAsync(InboxMessage message)
  {
    var jsonObject = JObject.Parse(message.Content);
    jsonObject.Remove("DomainEvents");
    message.Content = jsonObject.ToString();
    var user = JsonConvert.DeserializeObject<User>(message.Content, new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore });
    if (user == null)
    {
      throw new ValidationException("The Content of the message is not suitable for JSON.");
    }
    if (message.Type == "UserUpdateEvent")
    {
      var currentUser = _dbContext.Users.Where(u => u.Id == user.Id).ExecuteUpdate(setters =>
        setters.SetProperty(u => u.FirstName, user.FirstName)
               .SetProperty(u => u.LastName, user.LastName)
      );
    }
    else
    {
    _dbContext.Users.Add(user);
    }
    message.ProcessedOnUtc = DateTime.UtcNow;
    await _dbContext.SaveChangesAsync();

    return;
  }
}
