using Ardalis.Result;

namespace Socially.ContentManagment.Web.Extensions;

public static class ResultExtensions
{
  public static Microsoft.AspNetCore.Http.IResult ToProblemDetails<T>(this Result<T> result)
  {
    if (result.IsSuccess)
    {
      throw new InvalidOperationException("Can't convert success result to problem");
    }
    return Results.Problem(
      statusCode: StatusCodes.Status404NotFound,
      title: "Not Found",
      type: "https://datatracker.ietf.org/doc/html/rfc7231",
      extensions: new Dictionary<string, object?>
      {
        {"errors", new[] {result.Errors} }
      }
      );
  }
}
