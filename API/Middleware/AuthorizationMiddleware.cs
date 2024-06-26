using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized ||
                context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                var user = context.User;
                if (user.Identity.IsAuthenticated)

                {
                    var roles = user.Claims
                                    .Where(c => c.Type == ClaimTypes.Role)
                                    .Select(c => c.Value);

                    var roleInfo = roles.Any() ? string.Join(", ", roles) : "No roles assigned";
                    
                    var responseMessage = new
                    {
                        Status = context.Response.StatusCode,
                        Message = "You are not authorized to access this resource.",
                        Roles = roleInfo
                    };

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(responseMessage));
                }
            }
        }
    }
}
