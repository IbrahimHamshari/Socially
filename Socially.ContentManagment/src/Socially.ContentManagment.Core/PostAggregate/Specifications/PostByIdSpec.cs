using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Socially.ContentManagment.Core.PostAggregate.Specifications;
public class PostByIdSpec : Specification<Post>, ISingleResultSpecification<Post>
{
  public PostByIdSpec(Guid id)
  {
    Query.Where(post => post.Id == id);
  }
}
