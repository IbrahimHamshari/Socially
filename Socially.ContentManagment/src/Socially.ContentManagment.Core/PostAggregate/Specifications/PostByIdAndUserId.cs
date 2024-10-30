using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Socially.ContentManagment.Core.PostAggregate.Specifications;
public class PostByIdAndUserId : Specification<Post>, ISingleResultSpecification<Post>
{
  public PostByIdAndUserId(Guid postId, Guid userId)
  {
    Query.Where(p=> p.UserID == userId && p.Id == postId);
  }
}
