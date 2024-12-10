using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Socially.Messaging.Infrastructure.Data;
using Socially.Messaging.Infrastructure.Interfaces;
using Socially.Messaging.Infrastructure.Messaging;
using Socially.Messaging.Infrastructure.Messaging.Services;

namespace Socially.Messaging.Infrastructure;
public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger,
    bool isDevelopment = true
    )
  {
    string? connectionString = config.GetConnectionString("PostgreSqlConnection");
    Guard.Against.Null(connectionString);
    services.AddApplicationDbContext(connectionString);

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
    if(isDevelopment)
    {
    services.AddScoped(typeof(IRabbitMqConsumerService), typeof(RabbitMqConsumerService));
    services.Configure<RabbitMqConfiguration>(config.GetSection("RabbitMqConfiguration"));
    }
    services.AddSingleton(typeof(IUserIdProvider), typeof(UniqueNameUserIdProvider));
    services.AddScoped(typeof(INotificationService), typeof(SignalRNotificationService));
    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
