# NikiforovAll.CleanArchitecture.Template \[na-ca\]

## Technologies

* ASP.NET Core 6
* [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
* [MediatR](https://github.com/jbogard/MediatR)
* [AutoMapper](https://automapper.org/)
* [FluentValidation](https://fluentvalidation.net/)
* [XUnit](https://xunit.net/), [FluentAssertions](https://fluentassertions.com/), [Moq](https://github.com/moq), [AutoFixture](https://github.com/AutoFixture/AutoFixture), [Respawn](https://github.com/jbogard/Respawn)
* [Docker](https://www.docker.com/)

## Principles

* DDD
* Testability
* Separation of Concerns
* CQRS

![cleanarch](assets/DomainDrivenHexagon.png)
Reference to <https://github.com/Sairyss/domain-driven-hexagon>

## Overview

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### Api

This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only *Startup.cs* should reference Infrastructure.

## License

This project is licensed with the [Apache License](LICENSE).

## Credit

Inspired by:

* <https://github.com/JasonTaylorDev/CleanArchitecture>
* <https://github.com/ardalis/cleanarchitecture>
* <https://github.com/dotnet-architecture/eShopOnContainers>

## TODO


TODO: [X] ManagePackageVersionsCentrally
TODO: [X] Move logging to infra
TODO: [X] fix formatting in csproj file, should be consistent
TODO: [X] migrate to latest and greatest, e.g: fluent validation, dotnet-outdated
TODO: [X] add unit tests
TODO: [X] add integrations tests
TODO: [] add github ci
TODO: [] finish readme
TODO: [] dockerize
TODO: [X] cleanup appsettings.json
TODO: [] add mass transit integration example for worker (optional)
TODO: [] add example of application service, e.g: message broker publisher
TODO: [] clear warnings
TODO: [] ensure nullability (e.g: dtos)
TODO: [] dockerize integration tests
TODO: [] add diagram for solution structure, add docs about DDD, CQRS, Clean Architecture (typescript CA/DDD project has nice diagram)
TODO: [] add getting started guide: how to setup project, create and seed database, etc.
TODO: [] TreatWarningsAsErrors
