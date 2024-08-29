---
layout: default
title: Home
---

# About xUnit.net

[![.NET Foundation logo](https://raw.githubusercontent.com/xunit/media/main/dotnet-foundation.svg){: .float-right-100 }](https://dotnetfoundation.org/projects/project-detail/xunit)

xUnit.net is a free, open source, community-focused unit testing tool for the .NET Framework. Written by the original inventor of NUnit v2, xUnit.net is the latest technology for unit testing C#, F#, VB.NET and other .NET languages. xUnit.net works with ReSharper/Rider, CodeRush, TestDriven.NET and Xamarin. It is part of the [.NET Foundation](https://www.dotnetfoundation.org/), and operates under their [code of conduct](https://www.dotnetfoundation.org/code-of-conduct). It is licensed under [Apache 2](https://opensource.org/licenses/Apache-2.0) (an OSI approved license).

> _Follow: [xUnit.net on Mastodon](https://dotnet.social/@xunit){: rel="me" }, [James Newkirk](https://www.jamesnewkirk.com/), [Brad Wilson](https://bradwilson.io/),  [Claire Novotny](https://github.com/clairernovotny)_\\
> _Discussions are held on our [discussions site](https://github.com/xunit/xunit/discussions/)._\\
> _Resharper/Rider support is provided and supported by [JetBrains](https://www.jetbrains.com/)._\\
> _CodeRush support is provided and supported by [DevExpress](https://www.devexpress.com/)._\\
> _NCrunch support is provided and supported by [Remco Software](https://www.ncrunch.net/)._\\
> _The xUnit.net logo was designed by Nathan Young._


## Table of Contents

* [Current Releases](#current-releases)
* [Documentation](#documentation)
* [Test Runner Compatibility](#test-runner-compatibility)
* [Github Projects](#github-projects)
* [Links to Resources](#links-to-resources)
* [Sponsors](#sponsors)
* [Additional copyrights](#additional-copyrights)


## Current Releases

{: .table .latest }
|                           | Stable | Latest CI ([how to use](/docs/using-ci-builds)) | Build status
| ------------------------- | ------ | ---------------------------------------------------- | ------------
| v2 core framework         | [![](https://img.shields.io/nuget/v/xunit.svg?logo=nuget)](https://www.nuget.org/packages/xunit)                                         | [![](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit/latest)](https://feedz.io/org/xunit/repository/xunit/packages/xunit)                                         | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/xunit/badge%3Fref%3Dv2&amp;label=build)](https://actions-badge.atrox.dev/xunit/xunit/goto?ref=v2)
| v3 core framework         | [![](https://img.shields.io/nuget/v/xunit.v3.svg?logo=nuget)](https://www.nuget.org/packages/xunit.v3)                                   | [![](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit.v3/latest)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.v3)                                   | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/xunit/badge%3Fref%3Dmain&amp;label=build)](https://actions-badge.atrox.dev/xunit/xunit/goto?ref=main)
| xunit.analyzers           | [![](https://img.shields.io/nuget/v/xunit.analyzers.svg?logo=nuget)](https://www.nuget.org/packages/xunit.analyzers)                     | [![](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit.analyzers/latest)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.analyzers)                     | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/xunit.analyzers/badge%3Fref%3Dmain&amp;label=build)](https://actions-badge.atrox.dev/xunit/xunit.analyzers/goto?ref=main)
| xunit.runner.visualstudio | [![](https://img.shields.io/nuget/v/xunit.runner.visualstudio.svg?logo=nuget)](https://www.nuget.org/packages/xunit.runner.visualstudio) | [![](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit.runner.visualstudio/latest)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.runner.visualstudio) | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/visualstudio.xunit/badge%3Fref%3Dmain&amp;label=build)](https://actions-badge.atrox.dev/xunit/visualstudio.xunit/goto?ref=main)

### Latest Release Notes

{: .table .latest }
|                           | Stable                                             | Prerelease                                                          |
| ------------------------- | -------------------------------------------------- | ------------------------------------------------------------------- | ----------------------------------------
| v2 core framework         | [2.9.0](/releases/v2/2.9.0){: .release }           |                                                                     | ([all releases](/releases/v2/))
| v3 core framework         |                                                    | [0.2.0-pre.69](/releases/v3/0.2.0-pre.69){: .prerelease }           | ([all releases](/releases/v3/))
| xunit.analyzers           | [1.15.0](/releases/analyzers/1.15.0){: .release }  | [1.16.0-pre.22](/releases/analyzers/1.16.0-pre.22){: .prerelease }  | ([all releases](/releases/analyzers/))
| xunit.runner.visualstudio | [2.8.2](/releases/visualstudio/2.8.2){: .release } | [3.0.0-pre.24](/releases/visualstudio/3.0.0-pre.24){: .prerelease } | ([all releases](/releases/visualstudio/))


## Documentation

### Getting Started

_New to xUnit.net? These guides will help you get started._

{: .indent }
**<u>v2 Core Framework</u>** ([migrating from v1](/docs/getting-started/v2/migration))

{: .indent }
* **.NET Core & .NET 5+** &ndash; cross-platform applications, including ASP.NET Core
  * Using [command line](/docs/getting-started/v2/netcore/cmdline) (cross-platform)
  * Using [Visual Studio](/docs/getting-started/v2/netcore/visual-studio) (Windows)
* **.NET Framework** &ndash; Windows desktop &amp server applications, including ASP.NET
  * Using [command line](/docs/getting-started/v2/netfx/cmdline) (cross-platform)
  * Using [Visual Studio](/docs/getting-started/v2/netfx/visual-studio) (Windows)
  * Using [JetBrains Rider](/docs/getting-started/v2/netfx/jetbrains-rider) (cross-platform)

{: .indent }
**<u>v3 Core Framework</u>** ([migrating from v2](/docs/getting-started/v3/migration) and [what's new?](/docs/getting-started/v3/whats-new))

{: .indent }
_Content coming soon_

{: .indent }
**<u>Source Analyzers</u>**

{: .indent }
* [xUnit.net analyzer documentation](/xunit.analyzers/rules/)

### Configuration

* [Configuration files](/docs/configuration-files) (aka <code>xunit.runner.json</code>)
* [What is the JSON schema for <code>xunit.runner.json</code>?](/schema/)
* [RunSettings and <code>xunit.runner.visualstudio</code>](/docs/runsettings)

### Unit Test Parallelism

* [Running tests in parallel](/docs/running-tests-in-parallel)
* [Sharing context between tests](/docs/shared-context) (class, collection, and assembly fixtures)

### Frequently Asked Questions

* [Why did we build xUnit 1.0?](/docs/why-did-we-build-xunit-1.0)
* [What NuGet packages should I use?](/docs/nuget-packages)
* [How do I use a CI build NuGet package?](/docs/using-ci-builds)
* [What is the format of the XML generated by the test runners?](/docs/format-xml-v2)
* [How does xUnit.net compare to other frameworks?](/docs/comparisons)
* [How do I build xUnit.net?](https://github.com/xunit/xunit/blob/main/BUILDING.md)
* [Why doesn't xUnit.net support netstandard?](/docs/why-no-netstandard)
* [What is "theory data stability"?](/faq/theory-data-stability-in-vs)
* [Where do I find code that used to live in <code>xunit.extensions</code>?](/docs/upgrade-extensions)

### Other Topics

* [Multi-targeting on non-Windows OSes](/docs/getting-started/multi-target/non-windows)
* [Sample projects (including testing and extensibility)](https://github.com/xunit/samples.xunit)
* [Capturing output](/docs/capturing-output)
* [Equality with hash sets vs. linear containers](/docs/hash-sets-vs-linear-containers)
* [Running tests in MSBuild](/docs/running-tests-in-msbuild)
* [Getting Test Results in TeamCity](/docs/getting-test-results-in-teamcity)
* [Getting Test Results in CruiseControl.NET](/docs/getting-test-results-in-ccnet)
* [Getting Test Results in Azure DevOps](/docs/getting-test-results-in-azure-devops)


## Test Runner Compatibility

_The following target frameworks are current supported:_

{: .table .smaller }
|                                                      | xUnit.net | Console                | MSBuild                | TestDriven.NET <sup>2</sup> | Visual Studio <sup>3</sup>
| ---------------------------------------------------- | --------- | ---------------------- | ---------------------- | --------------------------- | --------------------------
| .NET Framework<br />(Windows)                        | 1.9.2+    | &#x2714; <sup>4a</sup> | &#x2714; <sup>4a</sup> | &#x2714; <sup>4a</sup>      | &#x2714; <sup>4b</sup>
| .NET Core &amp; .NET 5+<br />(Windows, Linux, macOS) | 2.2+      |                        |                        |                             | &#x2714; <sup>4b</sup>

_The following target frameworks have been deprecated and are no longer supported:_

{: .table .smaller }
|                                               | xUnit.net   | Visual Studio <sup>2</sup> | Devices
| --------------------------------------------- | ----------- | -------------------------- | -------
| Universal Application<br />(Win/WinPhone 8.1) | 2.0 - 2.1   | &#x2714; <sup>4b</sup>     | &#x2714; <sup>4c</sup>
| Universal Windows Platform                    | 2.1 - 2.4.2 | &#x2714; <sup>4b</sup>     | &#x2714; <sup>4c</sup>
| Windows Phone 8 (Silverlight)                 | 2.0 - 2.1   |                            | &#x2714; <sup>4c</sup>
| Xamarin iOS Unified <sup>1</sup>              | 2.0 - 2.4.2 |                            | &#x2714; <sup>4c</sup>
| Xamarin MonoAndroid <sup>1</sup>              | 2.0 - 2.4.2 |                            | &#x2714; <sup>4c</sup>
| Xamarin MonoTouch (iOS Classic) <sup>1</sup>  | 2.0 - 2.1   |                            | &#x2714; <sup>4c</sup>

{: .indent }
1. Requires Xamarin for Visual Studio or Xamarin Studio.
2. TestDriven.NET is no longer officially supported in v3 Core Framework, and is only available in Visual Studio 2019 and earlier.
3. Visual Studio support includes the Visual Studio Test Explorer, Visual Studio Code, <code>vstest.console.exe</code>, and <code>dotnet test</code>. Express editions of Visual Studio are not supported. Minimum target frameworks for are higher for v2 Core Framework users: NET Framework 4.6.2+ and .NET 6+.
4. Test runner source code availability:<br />
  a. [https://github.com/xunit/xunit](https://github.com/xunit/xunit)<br />
  b. [https://github.com/xunit/visualstudio.xunit](https://github.com/xunit/visualstudio.xunit)<br />
  c. [https://github.com/xunit/devices.xunit](https://github.com/xunit/devices.xunit)


## Github Projects

_For information on contributing to xUnit.net, please read the [governance](/governance) document._

* [xUnit.net](https://github.com/xunit/xunit) (core framework, built-in runners)
* [Assertion library](https://github.com/xunit/assert.xunit)
* [Analyzers](https://github.com/xunit/xunit.analyzers)
* [Visual Studio runner](https://github.com/xunit/visualstudio.xunit) (Visual Studio, Visual Studio Code, dotnet test)
* [Media files](https://github.com/xunit/media)
* [This site](https://github.com/xunit/xunit/tree/gh-pages)


## Links to Resources

* [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/)
* [.NET Framework 4.5.2](https://www.microsoft.com/en-us/download/details.aspx?id=42642)
* [TestDriven.NET](https://www.testdriven.net/)
* [MSBuild Reference](https://docs.microsoft.com/en-us/visualstudio/msbuild/msbuild-reference)

[![Powered by NDepend](https://raw.github.com/xunit/media/main/powered-by-ndepend-transparent.png){: width="142" }](https://www.NDepend.com)


## Sponsors

Special thanks to the [.NET on AWS Open Source Software Fund](https://github.com/aws/dotnet-foss) for sponsoring the ongoing development of xUnit.net.

{: .aws-logo }
[![Amazon Web Services logo](/images/aws-logo.svg)](https://github.com/aws/dotnet-foss)

Help support this project by becoming a sponsor through [GitHub Sponsors](https://github.com/sponsors/xunit).


## Additional copyrights

Portions copyright The Legion Of The Bouncy Castle