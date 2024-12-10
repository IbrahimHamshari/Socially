using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using SharedKernel.Constants;
using SharedKernel.CookieManagment;


namespace SharedKernel.Middleware;

public class RefreshMiddleware
{
    private readonly RequestDelegate _next;
    public RefreshMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();

            if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token); // Parse the token without validating it

                    // Extract the 'exp' claim
                    var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

                    if (expClaim != null && long.TryParse(expClaim, out var expUnix))
                    {
                        var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;

                        Console.WriteLine($"Token expires at: {expirationTime}");

                        if (expirationTime < DateTime.UtcNow)
                        {
                            var _cookieService = new CookieService(new HttpContextAccessor
                            {
                                HttpContext = context
                            });
                            var refreshToken = _cookieService.GetCookie("RefreshToken");
                            if (refreshToken != null)
                            {
                                context.Response.Redirect(URLConstants.REFRESHURL, true);
                                return;

                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to decode token: {ex.Message}");

                }


            }
        }
        await _next(context);
    }
}


public static class RefershMiddlewareExtension
{
    public static IApplicationBuilder UseRefresh(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RefreshMiddleware>();
    }
}