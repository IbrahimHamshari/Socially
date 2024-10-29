using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.ContentManagment.UseCases.Shares.Common.DTOs;
public class SharePostDto
{
  public Guid PostId { get; set; }

  public Guid Message {  get; set; }
}
