# This repo has been archived, source code and issues moved to [AppMetrics](https://github.com/AppMetrics/AppMetrics)

# App Metrics Prometheus <img src="https://avatars0.githubusercontent.com/u/29864085?v=4&s=200" alt="App Metrics" width="50px"/> 
[![Official Site](https://img.shields.io/badge/site-appmetrics-blue.svg?style=flat-square)](http://app-metrics.io/reporting/prometheus.html) [![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg?style=flat-square)](https://opensource.org/licenses/Apache-2.0)

## What is it?

This repo contains Prometheus extension packages to [App Metrics](https://github.com/AppMetrics/AppMetrics).

## Latest Builds, Packages & Repo Stats

|Branch|AppVeyor|Travis|Coverage|
|------|:--------:|:--------:|:--------:|
|dev|[![AppVeyor](https://img.shields.io/appveyor/ci/alhardy/prometheus/dev.svg?style=flat-square&label=appveyor%20build)](https://ci.appveyor.com/project/alhardy/prometheus/branch/dev)|[![Travis](https://img.shields.io/travis/alhardy/Prometheus/dev.svg?style=flat-square&label=travis%20build)](https://travis-ci.org/alhardy/Prometheus)|[![Coveralls](https://img.shields.io/coveralls/AppMetrics/Prometheus/dev.svg?style=flat-square)](https://coveralls.io/github/AppMetrics/Prometheus?branch=dev)
|master|[![AppVeyor](https://img.shields.io/appveyor/ci/alhardy/prometheus/master.svg?style=flat-square&label=appveyor%20build)](https://ci.appveyor.com/project/alhardy/prometheus/branch/master)| [![Travis](https://img.shields.io/travis/alhardy/Prometheus/master.svg?style=flat-square&label=travis%20build)](https://travis-ci.org/alhardy/Prometheus)| [![Coveralls](https://img.shields.io/coveralls/AppMetrics/Prometheus/master.svg?style=flat-square)](https://coveralls.io/github/AppMetrics/Prometheus?branch=master)|

|Package|Dev Release|PreRelease|Latest Release|
|------|:--------:|:--------:|:--------:|
|App.Metrics.Formatters.Prometheus|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Formatters.Prometheus.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Formatters.Prometheus)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Formatters.Prometheus.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Formatters.Prometheus/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Formatters.Prometheus.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Formatters.Prometheus/)

#### Grafana/Prometheus Web Monitoring

![Grafana/Prometheus Generic Web Dashboard Demo](https://github.com/AppMetrics/AppMetrics.DocFx/blob/master/images/generic_grafana_dashboard_demo.gif)

> Grab the dashboard [here](https://grafana.com/dashboards/2204)

### Grafana/Prometheus Web Application Setup

- Download and install [Prometheus](https://prometheus.io/docs/introduction/getting_started/). *Runs well on Windows using* `Bash on Windows on Ubuntu`
- Add a new scrape_config section to your `prometheus.yml` file

```
scrape_configs: 
  - job_name: 'appmetrics'
    
    scrape_interval: 5s   

    static_configs:
      - targets: ['localhost:1111'] #change this to your hostname, defaults to '/metrics'
```

- Download and install [Grafana](https://grafana.com/grafana/download), then create a new [Prometheus Datasource](http://docs.grafana.org/features/datasources/prometheus/) pointing the the Database just created and [import](http://docs.grafana.org/reference/export_import/#importing-a-dashboard) App.Metrics [web dashboard](https://grafana.com/dashboards/2204)
- See the [docs](https://www.app-metrics.io/reporting/reporters/prometheus/#asp-net-core-configuration) on configuring Prometheus with App Metrics
- Run your app and Grafana at visit `http://localhost:3000`

## How to build

[AppVeyor](https://ci.appveyor.com/project/alhardy/prometheus/branch/master) and [Travis CI](https://travis-ci.org/alhardy/Prometheus) builds are triggered on commits and PRs to `dev` and `master` branches.

See the following for build arguments and running locally.

|Configuration|Description|Default|Environment|Required|
|------|--------|:--------:|:--------:|:--------:|
|BuildConfiguration|The configuration to run the build, **Debug** or **Release** |*Release*|All|Optional|
|PreReleaseSuffix|The pre-release suffix for versioning nuget package artifacts e.g. `beta`|*ci*|All|Optional|
|CoverWith|**DotCover** or **OpenCover** to calculate and report code coverage, **None** to skip. When not **None**, a coverage file and html report will be generated at `./artifacts/coverage`|*OpenCover*|Windows Only|Optional|
|SkipCodeInspect|**false** to run ReSharper code inspect and report results, **true** to skip. When **true**, the code inspection html report and xml output will be generated at `./artifacts/resharper-reports`|*false*|Windows Only|Optional|
|BuildNumber|The build number to use for pre-release versions|*0*|All|Optional|
|LinkSources|[Source link](https://github.com/ctaggart/SourceLink) support allows source code to be downloaded on demand while debugging|*true*|All|Optional|


### Windows

Run `build.ps1` from the repositories root directory.

```
	.\build.ps1'
```

**With Arguments**

```
	.\build.ps1 --ScriptArgs '-BuildConfiguration=Release -PreReleaseSuffix=beta -CoverWith=OpenCover -SkipCodeInspect=false -BuildNumber=1'
```

### Linux & OSX

Run `build.sh` from the repositories root directory. Code Coverage reports are now supported on Linux and OSX, it will be skipped running in these environments.

```
	.\build.sh'
```

**With Arguments**

```
	.\build.sh --ScriptArgs '-BuildConfiguration=Release -PreReleaseSuffix=beta -BuildNumber=1'
```

## Contributing

See the [contribution guidlines](https://github.com/AppMetrics/AppMetrics/blob/master/CONTRIBUTING.md) in the [main repo](https://github.com/AppMetrics/AppMetrics) for details.

## Acknowledgements

* [DocFX](https://dotnet.github.io/docfx/)
* [Fluent Assertions](http://www.fluentassertions.com/)
* [XUnit](https://xunit.github.io/)
* [StyleCopAnalyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers)
* [Cake](https://github.com/cake-build/cake)
* [OpenCover](https://github.com/OpenCover/opencover)

***Thanks for providing free open source licensing***

* [Jetbrains](https://www.jetbrains.com/dotnet/) 
* [AppVeyor](https://www.appveyor.com/)
* [Travis CI](https://travis-ci.org/)
* [Coveralls](https://coveralls.io/)

## License

This library is release under Apache 2.0 License ( see LICENSE ) Copyright (c) 2016 Allan Hardy
