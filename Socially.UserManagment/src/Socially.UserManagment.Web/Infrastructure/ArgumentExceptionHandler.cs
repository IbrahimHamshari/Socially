using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Socially.UserManagment.Web.Infrastructure;

public class ArgumentExceptionHandler : IExceptionHandler
{
  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
  {
    if (exception is not ArgumentException)
      return false;

    var problemDetails = new ProblemDetails
    {
      Status = StatusCodes.Status400BadRequest,
      Title = "Bad Request",
      Type = "https://datatracker.ietf.org/doc/html/rfc7231",
      Extensions = new Dictionary<string, object?>
      {
        {"errors",new [] {exception.Message }}
      }
    };

    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

    return true;
  }
}
