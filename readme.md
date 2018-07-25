Ask questions or join the community using the [![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/dotless/dotless?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

Build status: [![Build status](https://ci.appveyor.com/api/projects/status/fx19i667vflulava?svg=true)](https://ci.appveyor.com/project/twenzel/dotless)

|NuGet package | Version information | Description
|- | - | -
|dotless.Core | [![NuGet](https://img.shields.io/nuget/v/dotless.Core.svg)](https://nuget.org/packages/dotless.Core/) | Less compiler
|dotless.AspNetHandler | [![NuGet](https://img.shields.io/nuget/v/dotless.AspNetHandler.svg)](https://nuget.org/packages/dotless.AspNetHandler/) | ASP.NET Handler to compile less files on the fly
|dotless | [![NuGet](https://img.shields.io/nuget/v/dotless.svg)](https://nuget.org/packages/dotless/) | Backward compatible package (containing Core and ASP.NET Handler). Please upgrade to either dotless.AspNetHandler or to dotless.Core if you don't need ASP.NET stuff
|dotless.CLI | [![NuGet](https://img.shields.io/nuget/v/dotless.CLI.svg)](https://nuget.org/packages/dotless.CLI/) | Command line interface (console application) to compile less files

.NET Framework version support
================
Starting with version 1.6 following .NET Framworks are suported:

* .NET Framework 4.5.1
* .NET Framework 4.6.1
* .NET Framework 4.7
* .NET Standard 2.0 (.NET Core 2.0, Mono 5.4, .NET Framework 4.6.1+)

If your application/library don't run on any of these Frameworks you have to stick with version 1.5.3.

Just Want a .dll?
=================

If you don't care about the source and just want a .dll you can get a compiled release from [Github](https://github.com/dotless/dotless/downloads).

Simply select for the latest successful build and click on the "Artifacts" section, here you'll find the latest compiler exe and any dll's required.

Or use the Core [NuGet package](https://nuget.org/packages/dotless.Core/).


Whats this all about?
---------------------

This is a project to port the hugely useful Less libary to the .NET world. 
It give variables, nested rules and operators to CSS. 

For more information about the original Less project see [http://lesscss.org/](http://lesscss.org/).
For more information about how to get started with the .NET version see  [http://www.dotlesscss.org/](http://www.dotlesscss.org).

ASP.NET Core Handler
--------------------
If you want to have a handler for ASP.NET Core applications please check out [WebOptimizer.Dotless](https://github.com/twenzel/WebOptimizer.Dotless).
