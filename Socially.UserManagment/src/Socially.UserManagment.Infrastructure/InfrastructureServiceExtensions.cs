using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Socially.UserManagment.Core.Interfaces;
using Socially.UserManagment.Core.Services;
using Socially.UserManagment.Infrastructure.Data;
using Socially.UserManagment.Infrastructure.Email;
using Socially.UserManagment.Infrastructure.Messaging;
using Socially.UserManagment.Infrastructure.Storage;
using Socially.UserManagment.Infrastructure.Token;
using Socially.UserManagment.UseCases.Interfaces;
using Socially.UserManagment.UseCases.Users.Interfaces;
using Supabase;

namespace Socially.UserManagment.Infrastructure;

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
    logger.LogInformation("{Project} services registered", nameof(Socially.UserManagment.Infrastructure));


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
