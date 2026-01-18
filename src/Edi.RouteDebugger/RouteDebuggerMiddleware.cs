using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;

namespace Edi.RouteDebugger;

public class RouteDebuggerMiddleware(RequestDelegate next, string path)
{
    private readonly RequestDelegate _next = next;
    private readonly string _path = path;

    public async Task Invoke(HttpContext context, IActionDescriptorCollectionProvider? provider)
    {
        if (context.Request.Path == _path)
        {
            await HandleRouteListRequest(context, provider);
        }
        else
        {
            await SetCurrentRouteInfoHeader(context);
        }
    }

    private static async Task HandleRouteListRequest(HttpContext context, IActionDescriptorCollectionProvider? provider)
    {
        if (provider is null)
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("IActionDescriptorCollectionProvider is not available", Encoding.UTF8);
            return;
        }

        var routes = provider.ActionDescriptors.Items.Select(x => new
        {
            Action = x.RouteValues.ContainsKey("Action") ? x.RouteValues["Action"] : null,
            Controller = x.RouteValues.ContainsKey("Controller") ? x.RouteValues["Controller"] : null,
            Page = x.RouteValues.ContainsKey("Page") ? x.RouteValues["Page"] : null,
            x.AttributeRouteInfo?.Name,
            x.AttributeRouteInfo?.Template,
            Constraint = x.ActionConstraints
        }).ToArray();

        var routesJson = JsonSerializer.Serialize(routes, new JsonSerializerOptions { WriteIndented = true });

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(routesJson, Encoding.UTF8);
    }

    private async Task SetCurrentRouteInfoHeader(HttpContext context)
    {
        // Set the header before calling next to avoid response buffering
        context.Response.OnStarting(() =>
        {
            var routeData = context.GetRouteData();
            if (routeData.Values.Count > 0)
            {
                var rdJson = JsonSerializer.Serialize(routeData.Values);
                context.Response.Headers["x-aspnet-route"] = rdJson;
            }
            return Task.CompletedTask;
        });

        await _next(context);
    }
}