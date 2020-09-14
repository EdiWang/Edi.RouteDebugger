using Edi.RouteDebugger;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RouteDebuggerExtensions
    {
        public static IApplicationBuilder UseRouteDebugger(this IApplicationBuilder app, string path = "/route-debugger")
        {
            return app.UseMiddleware<RouteDebuggerMiddleware>(path);
        }
    }
}