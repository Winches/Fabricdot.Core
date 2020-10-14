# Fabericdot Core

 A couple of libraries for building web application，aim to help people develop application with DDD principle in a simple way. 



## Using

- dotnet standard 2.1/dotnet core 3.1
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

### Fabericdot.Common.Core

Common Library

Provide basic functions and contracts.

### Fabericdot.Domain.Core

Domain Library

Build domain layer with abstraction of  `Entity`,`ValueObject`,`Enumeration`,`DomainService`,`Repository` and `Domain Event`.

### Fabericdot.Infrastructure.Core

Infrastructure Library

Provide basic implementation of persistence,CQRS and other  servies,etc.

### Fabericdot.Infrastructure.EntityFrameworkCore

Persistence Implementation with EntityFramework Core

### Fabericdot.WebApi.Core

Web Api Library with ASP.NET Core

Provide a couple features,like 'response filter',exception catching and api document.

## Usage

[Web API Template](https://github.com/Winches/Fabricdot.Template)