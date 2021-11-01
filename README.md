# NikiforovAll.CleanArchitecture.Templates

[![GitHub Actions Status](https://github.com/NikiforovAll/na-templates/workflows/Build/badge.svg?branch=main)](https://github.com/NikiforovAll/na-templates/actions)

[![GitHub Actions Build History](https://buildstats.info/github/chart/nikiforovall/na-templates?branch=main&includeBuildsFromPullRequest=false)](https://github.com/NikiforovAll/na-templates/actions)

A collection of templates for the rapid development of enterprise applications. (Clean Architecture, DDD, Event Sourcing)

## Install

```bash
dotnet new --install NikiforovAll.CleanArchitecture.Templates
```

* <https://github.com/NikiforovAll/na-ca>
* <https://github.com/NikiforovAll/na-es>
* <https://github.com/NikiforovAll/na-bu>

## Overview

The engine itself `dotnet new` provides information about possible configuration options.

```bash
dotnet new na-ca -h
dotnet new na-es -h
```

### Usage

The next command create default template in `my-project` folder.

`dotnet new na-ca -n my-project`

You can use `--dry-run` option to see what happens during command execution.

## Development

Run build pipeline: `dotnet cake`
