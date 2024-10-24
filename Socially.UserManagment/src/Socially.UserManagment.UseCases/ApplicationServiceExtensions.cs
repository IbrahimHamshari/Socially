using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Socially.ContentManagment.UseCases.Users.Login;

namespace Socially.ContentManagment.UseCases;

public static class ApplicationServiceExtensions
{
  public static IServiceCollection AddApplicationServices(
    this IServiceCollection services,
    ILogger logger)
  {
    services.AddScoped<ILoginService, LoginService>();
    logger.LogInformation("{Project} services registered", nameof(Socially.ContentManagment.UseCases));
    return services;
  }
}
