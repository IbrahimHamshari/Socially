using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Socially.UserManagment.Web.Infrastructure;

public class InternalExceptionHandler : IExceptionHandler
{
  private readonly ILogger<InternalExceptionHandler> _logger;

  public InternalExceptionHandler(ILogger<InternalExceptionHandler> logger)
  {
    _logger = logger;
  }

  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
  {
    if (exception is ArgumentException || exception is ValidationException || exception is ValidationException || (exception is DbUpdateException && exception.InnerException is PostgresException))
      return false;

    _logger.LogError(exception, "Exception Occured: {Message}", exception.Message);

    var problemDetails = new ProblemDetails
    {
      Status = StatusCodes.Status500InternalServerError,
      Title = "Server Error",
      Type = "https://datatracker.ietf.org/doc/html/rfc7231"
    };

    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

    return true;
  }
}
