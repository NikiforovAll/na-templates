# NikiforovAll.EventSourcing.Template \[na-es\]

## Technologies

* [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/)
* [FluentValidation](https://fluentvalidation.net/)
* [MediatR](https://github.com/jbogard/MediatR)
* [AutoMapper](https://automapper.org/)
* [MartenDB](https://martendb.io/)
* [MassTransit](https://masstransit-project.com/)
* [Docker](https://www.docker.com/)

Testing:

* [XUnit](https://xunit.net/)
* [FluentAssertions](https://fluentassertions.com/)
* [Moq](https://github.com/moq)
* [AutoFixture](https://github.com/AutoFixture/AutoFixture)
* [Respawn](https://github.com/jbogard/Respawn)

## Principles

* Event Sourcing
* CQRS
* DDD
* Testability
* Separation of Concerns

## Overview

![cleanarch](assets/DomainDrivenHexagon.png)
Reference to <https://github.com/Sairyss/domain-driven-hexagon>

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

* <https://github.com/oskardudycz/EventSourcing.NetCore>
* <https://github.com/JasonTaylorDev/CleanArchitecture>
* <https://github.com/ardalis/cleanarchitecture>
* <https://github.com/dotnet-architecture/eShopOnContainers>

## TODO

TODO: [X] implement es
TODO: [X] Add Console seeding implementation
TODO: [X] Add Marten.Console
TODO: [X] add github ci
TODO: [] finish readme
TODO: [X] dockerize
TODO: [X] add mass transit integration example for worker (optional)
TODO: [] add example of application service, e.g: message broker publisher
TODO: [X] clear warnings
TODO: [X] ensure nullability (e.g: dtos)
TODO: [] add diagram for solution structure, add docs about DDD, CQRS, Clean Architecture (typescript CA/DDD project has nice diagram)
TODO: [] add getting started guide: how to setup project, create and seed database, etc.
TODO: [X] TreatWarningsAsErrors
