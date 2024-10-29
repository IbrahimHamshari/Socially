using System.Reflection;
using Ardalis.ListStartupServices;
using Ardalis.SharedKernel;
using MediatR;
using Serilog;
using Serilog.Extensions.Logging;
using Socially.ContentManagment.Core.Interfaces;
using Socially.ContentManagment.Infrastructure;
using Socially.ContentManagment.Infrastructure.Data;
using Socially.ContentManagment.UseCases.Posts.Create;

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));
var microsoftLogger = new SerilogLoggerFactory(logger)
    .CreateLogger<Program>();

//builder.Services.AddQuartz(configure =>
//{
//  var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

//  configure
//    .AddJob<ProcessOutboxMessagesJob>(jobKey)
//    .AddTrigger(
//      trigger =>
//        trigger.ForJob(jobKey)
//          .WithSimpleSchedule(
//            schedule =>
//              schedule.WithIntervalInSeconds(10)
//                .RepeatForever()));

//});
// Configure Web Behavior
//builder.Services.AddQuartzHostedService();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => true;
  options.MinimumSameSitePolicy = SameSiteMode.None;
});

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JWTSettings>();

ConfigureMediatR();

builder.Services.AddInfrastructureServices(builder.Configuration, microsoftLogger);

if (builder.Environment.IsDevelopment())
{
  // Otherwise use this:
  //builder.Services.AddScoped<IEmailSender, FakeEmailSender>();
  AddShowAllServicesSupport();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseShowAllServicesMiddleware(); // see https://github.com/ardalis/AspNetCoreStartupServices
}
else
{
  app.UseHsts();
}


app.UseHttpsRedirection();

//await SeedDatabase(app);

app.Run();

//static async Task SeedDatabase(WebApplication app)
//{
//  using var scope = app.Services.CreateScope();
//  var services = scope.ServiceProvider;

//  try
//  {
//    var context = services.GetRequiredService<AppDbContext>();
//    //          context.Database.Migrate();
//    context.Database.EnsureCreated();
//    await SeedData.InitializeAsync(context);
//  }
//  catch (Exception ex)
//  {
//    var logger = services.GetRequiredService<ILogger<Program>>();
//    logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
//  }
//}

void ConfigureMediatR()
{
  var mediatRAssemblies = new[]
{
  Assembly.GetAssembly(typeof(Post)), // Core
  Assembly.GetAssembly(typeof(CreatePostCommand)) // UseCases
};
  builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!));
  builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
  builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
}

void AddShowAllServicesSupport()
{
  // add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
  builder.Services.Configure<ServiceConfig>(config =>
  {
    config.Services = new List<ServiceDescriptor>(builder.Services);

    // optional - default path to view services is /listallservices - recommended to choose your own path
    config.Path = "/listservices";
  });
}

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program
{
}
