using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.UserManagment.UseCases.Exceptions;
public class ExpiredTokenException : Exception
{
  public ExpiredTokenException():base("The refresh token has Expired")
  {
    
  }
}
