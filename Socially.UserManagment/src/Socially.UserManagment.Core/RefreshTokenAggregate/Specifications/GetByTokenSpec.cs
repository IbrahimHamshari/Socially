using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Socially.UserManagment.Core.RefreshTokenAggregate;

namespace Socially.UserManagment.Core.RefreshTokenAggregate.Specifications;
public class GetByTokenSpec : Specification<RefreshToken>
{
  public GetByTokenSpec(string token) 
  {
    Query.Where(t => t.Token == token);
  }
}
