using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.UserManagment.UseCases.Exceptions;
public class RevokedTokenException: Exception
{
  public RevokedTokenException(): base("The refresh token has been revoked.")
  {
    
  }
}
