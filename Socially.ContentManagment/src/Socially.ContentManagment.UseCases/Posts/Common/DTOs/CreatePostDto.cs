using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Socially.ContentManagment.Core.PostAggregate;

namespace Socially.ContentManagment.UseCases.Posts.Common.DTOs;
public class CreatePostDto
{
  public required string Content { get; set; }

  public IFormFile? Media { get; set; }

  public required int Privacy { get; set; }
}
