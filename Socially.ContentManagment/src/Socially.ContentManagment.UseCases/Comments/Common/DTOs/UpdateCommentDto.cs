using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.ContentManagment.UseCases.Comments.Common.DTOs;
public class UpdateCommentDto
{
  public Guid Id { get; set; }
  public required string Content { get; set; }

  public Guid PostId { get; set; }
}
