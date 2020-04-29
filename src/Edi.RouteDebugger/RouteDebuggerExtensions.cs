using Microsoft.AspNetCore.Builder;

namespace Edi.RouteDebugger
{
    public static class RouteDebuggerExtensions
    {
        public static IApplicationBuilder UseRouteDebugger(this IApplicationBuilder app)
        {
            app.UseMiddleware<RouteDebuggerMiddleware>();
            return app;
        }
    }
}
