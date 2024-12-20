﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;

namespace Socially.ContentManagment.Core.PostAggregate.Errors;
public static class CommentErrors
{
  public static Result NotFound(Guid commentId) => Result.NotFound("Comment.NotFound", $"The Comment with the Id = '{commentId}' was not found");

}
