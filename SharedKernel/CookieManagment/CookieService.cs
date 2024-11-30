using Microsoft.AspNetCore.Http;

namespace SharedKernel.CookieManagment;

public class CookieService : ICookieService
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public CookieService(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  // Set a cookie
  public void SetCookie(string key, string value, int? expireTime = null)
  {
    var options = new CookieOptions
    {
      HttpOnly = true,  // Helps prevent client-side JavaScript from accessing the cookie
      Secure = true,    // Only transmit cookies over HTTPS
      SameSite = SameSiteMode.Strict  // Helps mitigate CSRF attacks
    };

    if (expireTime.HasValue)
      options.Expires = DateTimeOffset.UtcNow.AddDays(expireTime.Value);
    else
      options.Expires = DateTimeOffset.UtcNow.AddDays(1); // Default expiration of 1 day

    _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, options);
  }

  // Get a cookie value
  public string? GetCookie(string key)
  {
    return _httpContextAccessor.HttpContext?.Request.Cookies[key];
  }

  // Remove a cookie
  public void RemoveCookie(string key)
  {
    _httpContextAccessor.HttpContext?.Response.Cookies.Delete(key);
  }
}
