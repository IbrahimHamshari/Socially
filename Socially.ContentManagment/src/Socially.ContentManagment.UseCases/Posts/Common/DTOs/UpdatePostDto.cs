using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Socially.ContentManagment.UseCases.Posts.Common.DTOs;
public class UpdatePostDto
{
  public required Guid Id { get; init; }

  public string? Content { get; init; }

  public IFormFile? Media { get; init; }

  public int? Privacy {  get; init; }

}
