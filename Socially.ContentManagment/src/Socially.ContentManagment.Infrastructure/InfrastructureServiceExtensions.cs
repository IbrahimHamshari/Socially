using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Socially.ContentManagment.Core.Interfaces;
using Socially.ContentManagment.Core.Services;
using Socially.ContentManagment.Infrastructure.Data;
using Socially.ContentManagment.Infrastructure.Data.Queries;
using Socially.ContentManagment.Infrastructure.Messaging;
using Socially.ContentManagment.UseCases.Contributors.List;

namespace Socially.ContentManagment.Infrastructure;
public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    string? connectionString = config.GetConnectionString("SqliteConnection");
    Guard.Against.Null(connectionString);
    services.AddApplicationDbContext(connectionString);

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
    services.AddScoped<IListContributorsQueryService, ListContributorsQueryService>();
    services.AddScoped<IDeleteContributorService, DeleteContributorService>();
    services.Configure<RabbitMqConfiguration>(config.GetSection("RabbitMqConfiguration"));

    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
