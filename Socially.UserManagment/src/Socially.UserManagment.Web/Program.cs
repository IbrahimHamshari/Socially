using System.Reflection;
using System.Text;
using Ardalis.ListStartupServices;
using Ardalis.SharedKernel;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Extensions.Logging;
using Socially.UserManagment.Core.Interfaces;
using Socially.UserManagment.Core.UserAggregate;
using Socially.UserManagment.Infrastructure;
using Socially.UserManagment.Infrastructure.CookieManagment;
using Socially.UserManagment.Infrastructure.Data;
using Socially.UserManagment.Infrastructure.Email;
using Socially.UserManagment.Shared.Config.JWT;
using Socially.UserManagment.UseCases;
using Socially.UserManagment.UseCases.Users.Register;
using Socially.UserManagment.UseCases.Validation;
using Socially.UserManagment.Web.Infrastructure;

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));
var microsoftLogger = new SerilogLoggerFactory(logger)
    .CreateLogger<Program>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
// Configure Web Behavior
builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => true;
  options.MinimumSameSitePolicy = SameSiteMode.None;
});

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JWTSettings>();

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
  });
builder.Services.Configure<JWTSettings>(
  builder.Configuration.GetSection("Jwt")
);
builder.Services.AddSingleton<ICookieService, CookieService>();

builder.Services.AddHttpContextAccessor();

ConfigureMediatR();

builder.Services.AddInfrastructureServices(builder.Configuration, microsoftLogger);
builder.Services.AddApplicationServices(microsoftLogger);
if (builder.Environment.IsDevelopment())
{
  // Use a local test email server
  // See: https://ardalis.com/configuring-a-local-test-email-server/
  builder.Services.AddScoped<IEmailSender, MimeKitEmailSender>();

  // Otherwise use this:
  //builder.Services.AddScoped<IEmailSender, FakeEmailSender>();
  AddShowAllServicesSupport();
}
else
{
  builder.Services.AddScoped<IEmailSender, MimeKitEmailSender>();
}

builder.Services.AddExceptionHandler<InternalExceptionHandler>();
builder.Services.AddExceptionHandler<ArgumentExceptionHandler>();

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

await SeedDatabase(app);

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();

static async Task SeedDatabase(WebApplication app)
{
  using var scope = app.Services.CreateScope();
  var services = scope.ServiceProvider;

  try
  {
    var context = services.GetRequiredService<AppDbContext>();
    //          context.Database.Migrate();
    context.Database.EnsureCreated();
    await SeedData.InitializeAsync(context);
  }
  catch (Exception ex)
  {
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
  }
}

void ConfigureMediatR()
{
  var mediatRAssemblies = new[]
{
  Assembly.GetAssembly(typeof(User)), // Core
  Assembly.GetAssembly(typeof(RegisterUserCommand)) // UseCases
};
  builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!));
  builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
  builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(RegisterUserCommandValidator)));
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
