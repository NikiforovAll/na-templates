# Coding Stories Template

[![GitHub Actions Status](https://github.com/NikiforovAll/na-templates/workflows/Build/badge.svg?branch=main)](https://github.com/NikiforovAll/na-templates/actions)

[![GitHub Actions Build History](https://buildstats.info/github/chart/nikiforovall/na-templates?branch=main&includeBuildsFromPullRequest=false)](https://github.com/NikiforovAll/na-templates/actions)

## Install

```bash
dotnet new --install CodingStories.Templates
```

## Overview

The engine itself `dotnet new` provides information about possible configuration options.

```bash
dotnet new na-ca -h
```

### Usage

The next command create default template in `my-project` folder.

`dotnet new na-ca -n my-project`

You can use `--dry-run` option to see what happens during command execution.

## Development

Run build pipeline: `dotnet cake`
