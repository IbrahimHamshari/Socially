﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;

namespace Socially.ContentManagment.Core.PostAggregate.Errors;
public static class PostErrors
{
  public static Result NotFound(Guid postId) => Result.NotFound("Posts.NotFound", $"The Post with the Id = '{postId}' was not found");
}