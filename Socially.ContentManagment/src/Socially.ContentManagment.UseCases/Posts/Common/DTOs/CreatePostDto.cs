using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Socially.ContentManagment.Core.PostAggregate;

namespace Socially.ContentManagment.UseCases.Posts.Common.DTOs;
public class CreatePostDto
{
  public required Guid Userid { get; set; }
  public required string Content { get; set; }

  public required string MediaURL { get; set; }

  public required Privacy Privacy { get; set; }
}
