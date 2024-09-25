﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Socially.UserManagment.Core.RefreshTokenAggregate.Specifications;
public class GetByFamilySpec : Specification<RefreshToken>
{
  public GetByFamilySpec(string family) 
  {
     Query.Where(t=> t.Family == family);  
  }
}