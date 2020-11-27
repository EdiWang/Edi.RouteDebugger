using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;

namespace Edi.RouteDebugger
{
    public class RouteDebuggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _path;

        public RouteDebuggerMiddleware(RequestDelegate next, string path)
        {
            _next = next;
            _path = path;
        }

        public Task Invoke(HttpContext context, IActionDescriptorCollectionProvider provider = null)
        {
            if (context.Request.Path == _path)
            {
                if (null != provider)
                {
                    var routes = provider.ActionDescriptors.Items.Select(x => new
                    {
                        Action = x.RouteValues.ContainsKey("Action") ? x.RouteValues["Action"] : null,
                        Controller = x.RouteValues.ContainsKey("Controller") ? x.RouteValues["Controller"] : null,
                        Page = x.RouteValues.ContainsKey("Page") ? x.RouteValues["Page"] : null,
                        x.AttributeRouteInfo?.Name,
                        x.AttributeRouteInfo?.Template,
                        Contraint = x.ActionConstraints
                    }).ToArray();

                    var routesJson = JsonSerializer.Serialize(routes, new JsonSerializerOptions() { WriteIndented = true });

                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(routesJson, Encoding.UTF8);
                }
                else
                {
                    return context.Response.WriteAsync("IActionDescriptorCollectionProvider is null", Encoding.UTF8);
                }
            }
            else
            {
                return SetCurrentRouteInfo(context);
            }
        }

        private async Task SetCurrentRouteInfo(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using var streamReader = new StreamReader(context.Response.Body);
            await streamReader.ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var routeData = context.GetRouteData();
            if (null != routeData)
            {
                // Each call of RouteData.Values property will create a new array-object, so cache the values here in order to reduce memory use & GC.
                // Details see: https://github.com/dotnet/aspnetcore/blob/master/src/Http/Http.Abstractions/src/Routing/RouteValueDictionary.cs
                var routeDataValues = routeData.Values;
                if (routeDataValues.Count > 0)
                {
                    var rdJson = JsonSerializer.Serialize(routeDataValues);
                    context.Response.Headers["current-route"] = rdJson;
                }
            }

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}