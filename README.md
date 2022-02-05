# Fabericdot Core

 A couple of libraries for building web application，aim to help people develop application with DDD principle in a simple way. 



## Using

- dotnet 5
- Entity Framework Core
- Dapper
- MediatR
- Ardalis.Sepcification
- AspectCore
- AutoMapper

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

### Fabericdot.Domain

[![NuGet](https://img.shields.io/nuget/v/Fabricdot.Domain.svg)](https://www.nuget.org/packages/Fabricdot.Domain)

Domain Library

Build domain layer with abstraction of  `Entity`,`ValueObject`,`Enumeration`,`DomainService`,`Repository` and `Domain Event`.

### Fabericdot.Infrastructure

[![NuGet](https://img.shields.io/nuget/v/Fabricdot.Infrastructure.svg)](https://www.nuget.org/packages/Fabricdot.Infrastructure)

Infrastructure Library

Provide basic implementation of persistence,CQRS and other  servies,etc.

### Fabericdot.Infrastructure.EntityFrameworkCore

[![NuGet](https://img.shields.io/nuget/v/Fabricdot.Infrastructure.EntityFrameworkCore.svg)](https://www.nuget.org/packages/Fabricdot.Infrastructure.EntityFrameworkCore)

Persistence Implementation with Entity Framework Core

### Fabericdot.WebApi

[![NuGet](https://img.shields.io/nuget/v/Fabricdot.WebApi.svg)](https://www.nuget.org/packages/Fabricdot.WebApi)

Web Api Library with ASP.NET Core

Provide a couple features,like 'response filter',exception catching and api document.

### Fabericdot.MultiTenancy

[![NuGet](https://img.shields.io/nuget/v/Fabricdot.MultiTenancy.svg)](https://www.nuget.org/packages/Fabricdot.MultiTenancy)

Multi-tenancy kit

Provide components to build multi-tenancy application.

## Usage

[Web API Template](https://github.com/Winches/Fabricdot.Template)