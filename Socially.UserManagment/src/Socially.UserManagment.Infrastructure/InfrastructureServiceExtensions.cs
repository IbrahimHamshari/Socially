using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Socially.ContentManagment.Core.Interfaces;
using Socially.ContentManagment.Core.Services;
using Socially.ContentManagment.Infrastructure.Data;
using Socially.ContentManagment.Infrastructure.Email;
using Socially.ContentManagment.Infrastructure.Messaging;
using Socially.ContentManagment.Infrastructure.Token;
using Socially.ContentManagment.UseCases.Interfaces;
using Socially.ContentManagment.UseCases.Users.Interfaces;
using Socially.UserManagment.Infrastructure.Storage;
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
    services.AddScoped<ICreateUserService, CreateUserService>();
    services.AddScoped<IDeleteUserService, DeleteUserService>();
    services.AddScoped<ITokenGenerator, TokenGenerator>();
    services.Configure<MailserverConfiguration>(config.GetSection("MailserverConfiguration"));
    services.AddScoped<IEmailSender, MimeKitEmailSender>();
    services.Configure<RabbitMqConfiguration>(config.GetSection("RabbitMqConfiguration"));
    services.AddSingleton<IRabbitMqService, RabbitMqService>();
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
