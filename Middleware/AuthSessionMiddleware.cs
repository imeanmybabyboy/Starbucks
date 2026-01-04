using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Starbucks.Middleware
{
    public class AuthSessionMiddleware
    {
        public const string SessionKey = "AuthSessionMiddleware";
        private readonly RequestDelegate _next;

        public AuthSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Query.ContainsKey("logout"))
            {
                context.Session.Remove(SessionKey);
                context.Response.Redirect(context.Request.Path);
                return;
            }


            if (context.Session.Keys.Contains(SessionKey))
            {
                var jsonUser = context.Session.GetString(SessionKey);
                Console.WriteLine(jsonUser);

                var user = JsonSerializer.Deserialize<Data.Entities.User>(jsonUser!)!;

                context.User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            [
                            new Claim(ClaimTypes.Name, user.Name),
                            new Claim(ClaimTypes.NameIdentifier, user.Email),
                            new Claim(ClaimTypes.DateOfBirth, user.Birthdate.ToString() ?? ""),
                            new Claim(ClaimTypes.Sid, user.Id.ToString()),
                            new Claim(ClaimTypes.Role, user.RoleId),
                            ],
                            nameof(AuthSessionMiddleware)));
            }

            await _next(context);
        }

        public static void SaveAuth(HttpContext context, Data.Entities.User user)
        {
            context.Session.SetString(AuthSessionMiddleware.SessionKey, JsonSerializer.Serialize(user));
        }

        public static void Logout(HttpContext context)
        {
            context.Session.Remove(AuthSessionMiddleware.SessionKey);
        }
    }

    public static class AuthSessionMiddlewareExtension
    {
        public static IApplicationBuilder UseAuthSession(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthSessionMiddleware>();
        }
    }
}
