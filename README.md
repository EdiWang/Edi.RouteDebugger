# ASP.NET Core RouteDebugger Middleware

![.NET Core](https://github.com/EdiWang/AspNetCore-RouteDebuggerMiddleware/workflows/.NET%20Core/badge.svg)

Show current route info and all routes in an ASP.NET Core application

## Background

Inspired by .NET Framework version of the origional RouteDebugger: https://haacked.com/archive/2008/03/13/url-routing-debugger.aspx/

We need something similar in .NET Core, with a few differences:

- Add route info in response header instead of append them in HTML
- Use JSON over HTML table for better tooling support
- Use Middleware to make it .NET Corelish

## Usage

### Install From NuGet

```bash
dotnet add package Edi.RouteDebugger
```

### Adding the Middleware

> Recommend use in development environment ONLY

> If you are using the Developer Exception Page middleware, put this middleware BEFORE the call to `app.UseDeveloperExceptionPage()` as the exception page would not work otherwise.

```csharp
if (env.IsDevelopment())
{
    app.UseRouteDebugger();
}
```

You can also use an overload to specify custom path where the route debugger will be available, for example:

```csharp
if (env.IsDevelopment())
{
    app.UseRouteDebugger("/tools/route-debugger");
}
```

### View Current Route

- Open any page in your application
- View response header `x-aspnet-route`

![](https://raw.githubusercontent.com/EdiWang/AspNetCore-RouteDebuggerMiddleware/master/screenshot/Screenshot_1.png)

### View All Routes

- Access `/route-debugger` or your custom path from browser or postman

![](https://raw.githubusercontent.com/EdiWang/AspNetCore-RouteDebuggerMiddleware/master/screenshot/Screenshot_2.png)
