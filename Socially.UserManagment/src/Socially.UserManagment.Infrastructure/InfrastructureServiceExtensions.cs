using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Socially.UserManagment.Core.Interfaces;
using Socially.UserManagment.Core.Services;
using Socially.UserManagment.Infrastructure.Data;
using Socially.UserManagment.Infrastructure.Data.Queries;
using Socially.UserManagment.Infrastructure.Email;
using Socially.UserManagment.UseCases.Users.Interfaces;
using Socially.UserManagment.Infrastructure.Token;
using Socially.UserManagment.UseCases.Contributors.List;

namespace Socially.UserManagment.Infrastructure;
public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    string? connectionString = config.GetConnectionString("DefaultConnection");
    Guard.Against.Null(connectionString);
    services.AddDbContext<AppDbContext>(options =>
     options.UseNpgsql(connectionString));

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
    services.AddScoped<IListContributorsQueryService, ListContributorsQueryService>();
    services.AddScoped<ICreateUserService, CreateUserService>();
    services.AddScoped<IDeleteUserService, DeleteUserService>();
    services.AddScoped<ITokenGenerator, TokenGenerator>();
    services.Configure<MailserverConfiguration>(config.GetSection("Mailserver"));

    logger.LogInformation("{Project} services registered", nameof(Socially.UserManagment.Infrastructure));

    return services;
  }
}
