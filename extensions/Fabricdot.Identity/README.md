# Fabricdot.Identtiy
Intergate ASP.NET Core Identity with DDD.

## Usage

```C#
            serviceCollection.AddIdentity<IdentityUser, IdentityRole>()
                 .AddRepositories<AppDbContext>()
                 .AddDefaultClaimsPrincipalFactory()
                 .AddDefaultTokenProviders();
```
