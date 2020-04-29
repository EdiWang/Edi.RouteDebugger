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

```csharp
if (env.IsDevelopment())
{
    app.UseRouteDebugger();
}
```

### View Current Route

- Open any page in your application
- View response header

### View All Routes

- Access ```/route-debugger``` from browser or postman
