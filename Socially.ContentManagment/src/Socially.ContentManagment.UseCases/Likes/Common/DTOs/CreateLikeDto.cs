using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.ContentManagment.UseCases.Likes.Common.DTOs;
public class CreateLikeDto
{
  public Guid PostId { get; set; }

  public Guid? CommentId {  get; set; }  

}
