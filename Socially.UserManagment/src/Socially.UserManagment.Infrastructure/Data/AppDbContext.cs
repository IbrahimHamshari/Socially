﻿using System.Reflection;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedKernel.Events;
using Socially.UserManagment.Core.RefreshTokenAggregate;
using Socially.UserManagment.Core.UserAggregate;
using Socially.UserManagment.Infrastructure.Data.Entites;

namespace Socially.UserManagment.Infrastructure.Data;

public class AppDbContext : DbContext
{
  private readonly IDomainEventDispatcher? _dispatcher;

  public AppDbContext(DbContextOptions<AppDbContext> options,
    IDomainEventDispatcher? dispatcher)
      : base(options)
  {
    _dispatcher = dispatcher;
  }

  public DbSet<User> Users => Set<User>();
  public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

  public DbSet<OutboxMessage> outboxMessages => Set<OutboxMessage>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("um");
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {

    var entitiesWithEvents = ChangeTracker.Entries<EntityBase<Guid>>()
    .Select(e => e.Entity)
    .Where(e => e.DomainEvents.Any())
    .ToArray();

   
    var outboxMessages = new List<OutboxMessage>();
    foreach (var entity in entitiesWithEvents)
    {
      foreach (var domainEvent in entity.DomainEvents)
      {
        var originalType = entity.GetType();
        if (domainEvent is not IOutboxEvent)
        {
          continue;
        }
        var outboxMessage = new OutboxMessage
        {
          Id = Guid.NewGuid(),
          OccuredOnUtc = DateTime.UtcNow,
          Type = originalType.Name!,
          Content = JsonConvert.SerializeObject(Convert.ChangeType(entity, originalType), new JsonSerializerSettings
          {
            TypeNameHandling = TypeNameHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
          }),  // Assuming JSON serialization
        };
        outboxMessages.Add(outboxMessage);
      }


    }
    await Set<OutboxMessage>().AddRangeAsync(outboxMessages, cancellationToken);


    int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    // ignore events if no dispatcher provided
    if (_dispatcher == null) return result;

    // dispatch events only if save was successful



    await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }

  public override int SaveChanges() =>
        SaveChangesAsync().GetAwaiter().GetResult();
}
