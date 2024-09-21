using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.UserManagment.Infrastructure.CookieManagment;
public interface ICookieService
{
  public void SetCookie(string key, string value, int? expireTime = null);
  public string? GetCookie(string key);
  public void RemoveCookie(string key);

}
