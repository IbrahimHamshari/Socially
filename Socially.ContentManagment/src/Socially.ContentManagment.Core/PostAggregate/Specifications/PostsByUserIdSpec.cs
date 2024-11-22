using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Socially.ContentManagment.Core.PostAggregate.Specifications;
public class PostsByUserIdSpec: Specification<Post>
{
  public PostsByUserIdSpec(Guid userId)
  {
    Query.Where(p => p.UserId == userId);
  }
}
