﻿using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Socially.ContentManagment.Infrastructure.Data;
using Socially.ContentManagment.Infrastructure.Messaging;
using Socially.ContentManagment.Infrastructure.Storage;
using Socially.ContentManagment.UseCases.Interfaces;
using Supabase;

namespace Socially.ContentManagment.Infrastructure;
public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    string? connectionString = config.GetConnectionString("PostgreSqlConnection");
    Guard.Against.Null(connectionString);
    services.AddApplicationDbContext(connectionString);

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
    services.AddScoped(typeof(IRabbitMqConsumerService), typeof(RabbitMqConsumerService));
    services.Configure<RabbitMqConfiguration>(config.GetSection("RabbitMqConfiguration"));
    logger.LogInformation("{Project} services registered", nameof(Socially.ContentManagment.Infrastructure));
    services.AddScoped<Supabase.Client>(_ => new Supabase.Client(
  config.GetSection("SupabaseUrl").Value!,
  config.GetSection("SupabaseKey").Value,
  new SupabaseOptions
  {
    AutoRefreshToken = true,
    AutoConnectRealtime = true
  }));
    services.AddScoped<IFileStorageService, SupabaseStorageService>();
    return services;
  }
}
