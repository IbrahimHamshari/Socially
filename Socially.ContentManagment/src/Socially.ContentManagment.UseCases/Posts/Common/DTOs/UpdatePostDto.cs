using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.ContentManagment.UseCases.Posts.Common.DTOs;
public class UpdatePostDto
{
  public required Guid Id { get; init; }

  public string? Content { get; init; }

  public string? MediaURL { get; init; }

  public int? Privacy {  get; init; }

}
