using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Socially.ContentManagment.Core.Constants;

namespace Socially.ContentManagment.Core.PostAggregate.Guards;
public static class CommentGuardExtension
{
  public static string InvalidContentFormat(this IGuardClause guardClause, string content, string parameterName)
  {
    return Guard.Against.InvalidFormat(content, parameterName, RegexConstants.CONTENT_REGEX);
  }
}
