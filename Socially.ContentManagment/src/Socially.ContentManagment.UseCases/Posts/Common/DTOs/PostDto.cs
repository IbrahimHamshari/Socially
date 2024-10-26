using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Socially.ContentManagment.UseCases.Posts.Common.DTOs;
public class PostDto
{
  public Guid? Id { get; init; }

  public Guid UserId { get; init; }

  public required string Content { get; init; }

  public string? MediaURL { get; init; }

  public required int Privacy {  get; init; }
}
