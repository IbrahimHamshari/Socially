using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Socially.UserManagment.Web.Infrastructure;

public class ArgumentValidationExceptionHandler : IExceptionHandler
{
  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
  {
    if (exception is not ArgumentException && exception is not ValidationException && exception is not ValidationException && (exception is DbUpdateException && exception.InnerException is not PostgresException) && exception is not NotSupportedException)
      return false;
    var message = "";
    if((exception is DbUpdateException && exception.InnerException is PostgresException))
    {
      var innerException = exception!.InnerException! as PostgresException;
      message = innerException!.MessageText;
    }
    else
    {
      message = exception.Message;
    }
    var problemDetails = new ProblemDetails
    {
      Status = StatusCodes.Status400BadRequest,
      Title = "Bad Request",
      Type = "https://datatracker.ietf.org/doc/html/rfc7231",
      Extensions = new Dictionary<string, object?>
      {
        {"errors",new [] {message }}
      }
    };

    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

    return true;
  }
}
