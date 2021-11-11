# Fabericdot Core

 A couple of libraries for building web application，aim to help people develop application with DDD principle in a simple way. 



## Using

- dotnet 5
- Entity Framework Core
- Dapper
- MediatR
- Ardalis.Sepcification

## Pattern

- Domain Event
- Repository
- Unit of work
- Specification
- CQRS



## Packages

### Fabericdot.Core

[![NuGet](https://img.shields.io/nuget/v/Fabricdot.Core.svg)](https://www.nuget.org/packages/Fabricdot.Core)

Core Library

Provide basic functions and contracts.

### Fabericdot.Domain.Core

[![NuGet](https://img.shields.io/nuget/v/Fabricdot.Domain.Core.svg)](https://www.nuget.org/packages/Fabricdot.Domain.Core)

Domain Library

Build domain layer with abstraction of  `Entity`,`ValueObject`,`Enumeration`,`DomainService`,`Repository` and `Domain Event`.

### Fabericdot.Infrastructure.Core

[![NuGet](https://img.shields.io/nuget/v/Fabricdot.Infrastructure.Core.svg)](https://www.nuget.org/packages/Fabricdot.Infrastructure.Core)

Infrastructure Library

Provide basic implementation of persistence,CQRS and other  servies,etc.

### Fabericdot.Infrastructure.EntityFrameworkCore

[![NuGet](https://img.shields.io/nuget/v/Fabricdot.Infrastructure.EntityFrameworkCore.svg)](https://www.nuget.org/packages/Fabricdot.Infrastructure.EntityFrameworkCore)

Persistence Implementation with EntityFramework Core

### Fabericdot.WebApi.Core

[![NuGet](https://img.shields.io/nuget/v/Fabricdot.WebApi.Core.svg)](https://www.nuget.org/packages/Fabricdot.WebApi.Core)

Web Api Library with ASP.NET Core

Provide a couple features,like 'response filter',exception catching and api document.

## Usage

[Web API Template](https://github.com/Winches/Fabricdot.Template)