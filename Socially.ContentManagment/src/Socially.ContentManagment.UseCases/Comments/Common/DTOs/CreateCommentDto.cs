﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.ContentManagment.UseCases.Comments.Common.DTOs;
public class CreateCommentDto
{
  public Guid PostId { get; set; }

  public required string Content {  get; set; }

}
