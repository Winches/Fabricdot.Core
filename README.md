# Fabericdot Core
[![nuget](https://img.shields.io/nuget/v/Fabricdot.Core.svg?style=flat-square&color=blue)](https://www.nuget.org/packages/Fabricdot.Core)
![build](https://img.shields.io/github/workflow/status/Winches/Fabricdot.Core/CI?style=flat-square)
![codecov](https://img.shields.io/codecov/c/gh/Winches/Fabricdot.Core?branch=dev&style=flat-square&token=615Z4TKL1D)
![license](https://img.shields.io/github/license/winches/Fabricdot.Core?style=flat-square)

`Fabricdot` is designed to simplify and shorten development of application with `DDD` concept.It aim to make development easier while using `DDD`.

## Getting Started

### Installation

You can install the following packages to create your first project:

```
PM> Install-Package Fabricdot.Core
PM> Install-Package Fabricdot.Domain
PM> Install-Package Fabricdot.Infrastructure
PM> Install-Package Fabricdot.Infrastructure.EntityFrameworkCore
PM> Install-Package Fabricdot.WebApi
PM> Install-Package Fabricdot.Authorization
PM> Install-Package Fabricdot.MultiTenancy
PM> Install-Package Fabricdot.MultiTenancy.AspNetCore
```

Fabricdot provides some useful extensions,following packages are available to install:

```
PM> Install-Package Fabricdot.Identity.Domain;
PM> Install-Package Fabricdot.Identity;
PM> Install-Package Fabricdot.PermissionGranting;
```

### Usage

Here we'll use [hexagonal-architecture](https://en.wikipedia.org/wiki/Hexagonal_architecture_(software)) to build a web application.

#### Define Domain Layer

Create domain models:

```c#
    using Fabricdot.Domain.Entities;
    using Fabricdot.Domain.Services;
    using Fabricdot.Domain.SharedKernel;
    using Fabricdot.Domain.ValueObjects;
    
    // Aggregate root
    public class Order : FullAuditAggregateRoot<Guid>
    {
        private readonly List<OrderLine> _orderLines = new();
        public IReadOnlyCollection<OrderLine> OrderLines => _orderLines.AsReadOnly();

        public Money Total { get; private set; }

        public DateTime OrderTime { get; private set; }

        public OrderStatus OrderStatus { get; private set; }

        public Address ShippingAddress { get; private set; }

        public Guid CustomerId { get; private set; }

        // ...
        
        internal Order(
            Guid id,
            Address shippingAddress,
            Guid customerId)
        {
            Id = Guard.Against.Default(id, nameof(id));
            ShippingAddress = Guard.Against.Null(shippingAddress, nameof(shippingAddress));
            CustomerId = Guard.Against.Default(customerId, nameof(customerId));
            OrderTime = SystemClock.Now;
            OrderStatus = OrderStatus.Placed;
            Total = Money.Zero;
        }

        private Order()
        {
            // For orm purpose.
        }

        public void AddOrderLine(
            Guid productId,
            int quantity,
            Money price)
        {
            if (OrderStatus != OrderStatus.Placed)
                throw new DomainException("Order can not be modified.");

            var orderLine = _orderLines.SingleOrDefault(v => v.ProductId == productId);
            if (orderLine == null)
                _orderLines.Add(new OrderLine(productId, quantity, price));
            else
                orderLine.AddQuantity(quantity);

            Total = Calculate(_orderLines);
        }

        private static Money Calculate(ICollection<OrderLine> orderLines)
        {
            return orderLines.Sum(v => v.Price * v.Quantity);
        }
    }

    // Entity
    public class OrderLine : Entity<Guid>
    {
        public Guid ProductId { get; private set; }

        public int Quantity { get; private set; }

        public Money Price { get; private set; }

        internal OrderLine(
            Guid productId,
            int quantity,
            Money price)
        {
            ProductId = Guard.Against.Default(productId, nameof(productId));
            Quantity = Guard.Against.NegativeOrZero(quantity, nameof(quantity));
            Price = Guard.Against.Null(price, nameof(price));
        }

        private OrderLine()
        {
        }
    }

    // Value object
    public class Address : ValueObject
    {
        public string Country { get; private set; }

        public string State { get; private set; }

        public string City { get; private set; }

        public string Street { get; private set; }

        private Address()
        {
        }

        public Address(
            string country,
            string state,
            string city,
            string street)
        {
            Country = Guard.Against.NullOrEmpty(country, nameof(country));
            State = Guard.Against.NullOrEmpty(state, nameof(state));
            City = Guard.Against.NullOrEmpty(city, nameof(city));
            Street = Guard.Against.NullOrEmpty(street, nameof(street));
        }
    }

    // Enumeration
    public class OrderStatus : Enumeration
    {
        public static OrderStatus Placed = new OrderStatus(1, nameof(Placed).ToLowerInvariant());

        public static OrderStatus Shipped = new OrderStatus(2, nameof(Shipped).ToLowerInvariant());

        public static OrderStatus Completed = new OrderStatus(3, nameof(Completed).ToLowerInvariant());

        protected OrderStatus(
            int value,
            string name) : base(value, name)
        {
        }

        public static implicit operator OrderStatus(int value) => FromValue<OrderStatus>(value);
    }

    // Domain service
    public interface IOrderService : IDomainService
    {
        Order Create(
            Address shippingAddress,
            Guid customerId);
    }
```

Create repository contract for your aggregare root:

```c#
    using Fabricdot.Domain.Services;
    
    public interface IOrderRepository : IRepository<Order,Guid>
    {
    }
```

Define module for your domain layer:

```c#
    using Fabricdot.Core.Modularity;
    
    [Requires(typeof(FabricdotDomainModule))]
    [Exports]
    public class SampleDomainModule : ModuleBase
    {
    }
```


#### Define infrastructure layer

Create dbcontext for your application:

```c#
    using Fabricdot.Infrastructure.EntityFrameworkCore;

    public class AppDbContext : DbContextBase
    {
        public DbSet<Order> Orders => Set<Order>();
        
        public AppDbContext ([NotNull] DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
```

Implement repository with repository contract:

```c#
    using Fabricdot.Infrastructure.EntityFrameworkCore;

    internal class OrderRepository : EfRepository<AppDbContext , Order, int>, IOrderRepository 
    {
        public OrderRepository (IDbContextProvider<AppDbContext > dbContextProvider) : base(dbContextProvider)
        {
        }
    }
```

Implement domain service with service contract:

```c#
    internal class OrderService : IOrderService
    {
        private readonly IGuidGenerator _guidGenerator;

        public OrderService(IGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
        }

        public Order Create(
            Address shippingAddress,
            Guid customerId)
        {
            var id = _guidGenerator.Create();
            return new Order(id, shippingAddress, customerId);
        }
    }
```

Define module for infrastructure layer:

```c#
    using Fabricdot.Core.Modularity;

    [Requires(typeof(SampleDomainModule))]
    [Requires(typeof(FabricdotInfrastructureModule))]
    [Requires(typeof(FabricdotEntityFrameworkCoreModule))]
    [Exports]
    public class SampleInfrastructureModule : ModuleBase
    {
        public override ConfigureServices(ConfigureServiceContext context)
        {
            var services = context.Services;
            services.AddEfDbContext<AppDbContext>((_, opts) =>
            {
                // use database provider.
            });
        }
    }
```

#### Define application layer

Create command and handler for your task:

```c#
    using Fabricdot.Infrastructure.Commands;

    // Command
    public class PlaceOrderCommand: CommandBase
    {
        [Required]
        [MinLength(1)]
        public List<OrderLineDto> OrderLines { get;set; }
        //...
    }

    // Command handler
    internal class PlaceOrderCommandHandler: ICommandHandler<PlaceOrderCommand>
    {
        private readonly IOrderService _orderService;
        private readonly IOrderRepository _orderRepository;

        public PlaceOrderCommandHandler(
            IOrderService orderService,
            IOrderRepository orderRepository)
        {
            _orderService = orderService;
            _orderRepository = orderRepository
        }

        public async Task<Unit> Handle(
            PlaceOrderCommand request,
            CancellationToken cancellationToken)
        {
            var order = _orderService.Create(request.Address,request.CustomerId);
            await _orderRepository.AddAsync(order,cancellationToken);
        }
    }
```

Create api controller

```c#
    using Fabricdot.WebApi.Endpoint;

    public class OrderController : EndPointBase
    {
        [HttpPost]
        public async Task CreateAsync([FromBody] PlaceOrderCommand command) => await Sender.Send(command);
    }
```

Module

```c#
    using Fabricdot.Core.Modularity;
    using Fabricdot.WebApi.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    [Requires(typeof(SampleInfrastructureModule))]
    [Requires(typeof(FabricdotWebApiModule))]
    [Exports]
    public class SampleApplicationModule : ModuleBase
    {
        public override ConfigureServices(ConfigureServiceContext context)
        {
            var services = context.Services;
            services.AddControllers(opts =>
            {
                opts.AddActionFilters();
            })
                .ConfigureApiBehaviorOptions(opts =>
                {
                    opts.SuppressModelStateInvalidFilter = true;
                });
        }

        public override OnStartingAsync(ApplicationStartingContext context)
        {
            var app = context.ServiceProvider.GetApplicationBuilder();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
```

Modify `Startup.cs` 

```c#
    using Fabricdot.Core.Boot;
    using Microsoft.AspNetCore.Builder;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBootstrapper<SampleApplicationModule>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Bootstrap();
        }
    }

```

Modify `Programm.cs`

```c#
    using Fabricdot.Infrastructure.DependencyInjection;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().StartAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                       .UseServiceProviderFactory(new FabricdotServiceProviderFactory());
        }
    }
```

Finally we can use `dotnet run` to see the web host.

## Other Link

[Web API Template](https://github.com/Winches/Fabricdot.Template)