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


## 免责申明

此项目(Edi.RouteDebugger)及其配套组件均为免费开源的产品，仅用于学习交流，并且不直接向中国提供服务，中国用户请于下载后立即删除。

任何中国境内的组织及个人不得使用此项目(Edi.RouteDebugger)及其配套组件构建任何形式的面向中国境内用户的网站或服务。

不可用于任何违反中华人民共和国(含台湾省)或使用者所在地区法律法规的用途。

因为作者即本人仅完成代码的开发和开源活动(开源即任何人都可以下载使用)，从未参与用户的任何运营和盈利活动。

且不知晓用户后续将程序源代码用于何种用途，故用户使用过程中所带来的任何法律责任即由用户自己承担。

[《开源软件有漏洞，作者需要负责吗？是的！》](https://go.edi.wang/aka/os251)