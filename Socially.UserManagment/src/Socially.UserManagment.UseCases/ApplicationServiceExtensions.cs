using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Socially.UserManagment.UseCases.Users.Login;

namespace Socially.UserManagment.UseCases;

public static class ApplicationServiceExtensions
{
  public static IServiceCollection AddApplicationServices(
    this IServiceCollection services,
    ILogger logger)
  {
    services.AddScoped<ILoginService, LoginService>();
    logger.LogInformation("{Project} services registered", nameof(Socially.UserManagment.UseCases));
    return services;
  }
}
