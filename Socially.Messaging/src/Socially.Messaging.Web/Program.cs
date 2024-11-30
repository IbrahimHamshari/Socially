using System.Reflection;
using System.Text;
using Ardalis.ListStartupServices;
using Ardalis.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Quartz;
using Serilog;
using Serilog.Extensions.Logging;
using SharedKernel.Middleware;
using Socially.Messaging.Core.MessageAggregate;
using Socially.Messaging.Infrastructure;
using Socially.Messaging.Infrastructure.BackgroundJobs;
using Socially.Messaging.Infrastructure.Data;
using Socially.Messaging.Infrastructure.Messaging;
using Socially.Messaging.UseCases.Messages.Send;
using Socially.Messaging.Web.Infrastructure;
using Socially.SharedKernel.Config.JWT;


var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));
var microsoftLogger = new SerilogLoggerFactory(logger)
    .CreateLogger<Program>();

// Configure Web Behavior
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
  options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => true;
  options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddSwaggerGen();

ConfigureMediatR();
builder.Services.AddSignalR();


var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JWTSettings>();
builder.Services.AddControllers();

builder.Services.AddAuthentication()
  .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
  {
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidIssuer = jwtOptions!.Issuer,
      ValidateAudience = true,
      ValidAudience = jwtOptions.Audience,
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
    };
    options.Events = new JwtBearerEvents
    {
      OnMessageReceived = context =>
      {
        var accessToken = context.Request.Query["access_token"];
        var path = context.HttpContext.Request.Path;
        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hub/chat"))
        {
          // Assign the token to the context for authentication
          context.Token = accessToken;
        }
          return Task.CompletedTask;
      }
    };
  });


builder.Services.Configure<JWTSettings>(
  builder.Configuration.GetSection("Jwt")
);
builder.Services.AddHttpContextAccessor();

if (builder.Environment.IsDevelopment())
{

  builder.Services.AddSwaggerGen();

  AddShowAllServicesSupport();

}
else
{

  builder.Services.AddQuartz(configure =>
  {
    var jobKey = new JobKey(nameof(ProcessRabbitMQMessagesJob));

    configure
      .AddJob<ProcessRabbitMQMessagesJob>(jobKey)
      .AddTrigger(
        trigger =>
          trigger.ForJob(jobKey)
            .WithSimpleSchedule(
              schedule =>
                schedule.WithIntervalInSeconds(10)
                  .RepeatForever()));

    var inboxJobKey = new JobKey(nameof(ProcessInboxMessagesJob));

    configure
      .AddJob<ProcessInboxMessagesJob>(inboxJobKey)
      .AddTrigger(
        trigger =>
          trigger.ForJob(inboxJobKey)
            .WithSimpleSchedule(
              schedule =>
                schedule.WithIntervalInSeconds(10)
                  .RepeatForever()));
  });
  builder.Services.AddQuartzHostedService();
}
builder.Services.AddInfrastructureServices(builder.Configuration, microsoftLogger);

builder.Services.AddExceptionHandler<InternalExceptionHandler>();
builder.Services.AddExceptionHandler<ArgumentValidationExceptionHandler>();

builder.Services.AddProblemDetails();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseShowAllServicesMiddleware(); // see https://github.com/ardalis/AspNetCoreStartupServices

  app.UseSwagger();
  app.UseSwaggerUI();

}
else
{
  app.UseHsts();
}


app.UseHttpsRedirection();

//await SeedDatabase(app);

app.UseRefresh();

app.UseAuthentication();
app.UseAuthorization();


app.UseExceptionHandler();

app.MapHub<ChatHub>("/api/chatHub").RequireAuthorization();

app.MapControllers();

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
  Assembly.GetAssembly(typeof(Message)), // Core
  Assembly.GetAssembly(typeof(SendMessageCommand)) // UseCases
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
