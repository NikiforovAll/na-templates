# NikiforovAll.CleanArchitecture.Templates

[![GitHub Actions Status](https://github.com/NikiforovAll/na-templates/workflows/Build/badge.svg?branch=main)](https://github.com/NikiforovAll/na-templates/actions)
[![NuGet](https://img.shields.io/nuget/dt/NikiforovAll.CleanArchitecture.Templates.svg)](http://nuget.org/packages/NikiforovAll.CleanArchitecture.Templates)
[![NuGet](https://img.shields.io/nuget/v/NikiforovAll.CleanArchitecture.Templates.svg)](https://www.nuget.org/packages/NikiforovAll.CleanArchitecture.Templates/)
[![NuGet](https://img.shields.io/nuget/vpre/NikiforovAll.CleanArchitecture.Templates.svg)](https://www.nuget.org/packages/NikiforovAll.CleanArchitecture.Templates/)

[![GitHub Actions Build History](https://buildstats.info/github/chart/nikiforovall/na-templates?branch=main&includeBuildsFromPullRequest=false)](https://github.com/NikiforovAll/na-templates/actions)

| Template | Status                                                                                                                                                      |
|----------|-------------------------------------------------------------------------------------------------------------------------------------------------------------|
| na-ca    | [![.NET](https://github.com/NikiforovAll/na-ca/actions/workflows/dotnet.yml/badge.svg)](https://github.com/NikiforovAll/na-ca/actions/workflows/dotnet.yml) |
| na-es    | [![.NET](https://github.com/NikiforovAll/na-es/actions/workflows/dotnet.yml/badge.svg)](https://github.com/NikiforovAll/na-es/actions/workflows/dotnet.yml) |
| na-ga    | [![.NET](https://github.com/NikiforovAll/na-ga/actions/workflows/dotnet.yml/badge.svg)](https://github.com/NikiforovAll/na-ga/actions/workflows/dotnet.yml) |
| na-bu    | N/A                                                                                                                                                         |

---

> A collection of templates for the rapid development of enterprise applications. (Clean Architecture, DDD, Event Sourcing)

## Install

```bash
dotnet new --install NikiforovAll.CleanArchitecture.Templates
```

Once installed, you can see a list of templates by running:

```bash
$ dotnet new -l na-
# These templates matched your input: 'na-'

# Template Name                Short Name  Language  Tags
# ---------------------------  ----------  --------  --------------------------------------------
# Build Project Template       na-bu       bash      build-project/Template
# Clean Architecture Template  na-ca       [C#]      CleanArchitecture/DDD/Template
# Event Sourcing Template      na-es       [C#]      EventSourcing/CleanArchitecture/DDD/Template
# Gateway Template             na-ga       [C#]      gateway/Template
```

* <https://github.com/NikiforovAll/na-ca>
* <https://github.com/NikiforovAll/na-es>
* <https://github.com/NikiforovAll/na-bu>
* <https://github.com/NikiforovAll/na-ga>

## Overview

The engine itself `dotnet new` provides information about possible configuration options.

```bash
dotnet new na-bu -h
dotnet new na-ca -h
dotnet new na-es -h
dotnet new na-ga -h
```

### Usage

The next command create default template in `my-project` folder.

`dotnet new na-ca -n my-project`

You can use `--dry-run` option to see what happens during command execution.

## Development

Run build pipeline: `dotnet cake`
