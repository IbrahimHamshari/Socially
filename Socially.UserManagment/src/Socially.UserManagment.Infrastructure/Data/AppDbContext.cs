using System.Reflection;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedKernel.Events;
using SharedKernel.Messages;
using Socially.UserManagment.Core.RefreshTokenAggregate;
using Socially.UserManagment.Core.UserAggregate;

namespace Socially.UserManagment.Infrastructure.Data;

public class AppDbContext : DbContext
{
  private readonly IDomainEventDispatcher? _dispatcher;
  private readonly ILogger<AppDbContext> _logger;
  public AppDbContext(DbContextOptions<AppDbContext> options,
    IDomainEventDispatcher? dispatcher,
    ILogger<AppDbContext> logger)
      : base(options)
  {
    _dispatcher = dispatcher;
    _logger = logger;
  }

  public DbSet<User> Users => Set<User>();
  public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

  public DbSet<OutboxMessage> outboxMessages => Set<OutboxMessage>();

  public DbSet<UserConnection> userConnections => Set<UserConnection>();
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
                                           .Where(e => e.DomainEvents != null && e.DomainEvents.Any())
                                           .ToArray();

    using (var transaction = await this.Database.BeginTransactionAsync(cancellationToken))
    {
      try
      {
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        if (result > 0 && _dispatcher != null)
        {
          var outboxMessages = entitiesWithEvents
              .SelectMany(entity => entity.DomainEvents.OfType<IOutboxEvent>().Select(domainEvent =>
                  new OutboxMessage
                  {
                    Id = Guid.NewGuid(),
                    OccuredOnUtc = DateTime.UtcNow,
                    Type = domainEvent.GetType().FullName!,
                    Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                    {
                      TypeNameHandling = TypeNameHandling.All,
                      ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    })
                  }))
              .ToList();

          await Set<OutboxMessage>().AddRangeAsync(outboxMessages, cancellationToken);
          await base.SaveChangesAsync(cancellationToken);

          await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);
        }

        await transaction.CommitAsync(cancellationToken);
        return result;
      }
      catch (Exception ex)
      {
        await transaction.RollbackAsync(cancellationToken);
        _logger.LogError(ex, "An error occurred during SaveChangesAsync. Transaction rolled back.");
        throw;
      }
    }
  }


  public override int SaveChanges() =>
        SaveChangesAsync().GetAwaiter().GetResult();
}
