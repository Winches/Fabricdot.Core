# Fabricdot.PermissionGranting

Implement permission granting feature.

## Usage

1.Import module `FabricdotPermissionGrantingModule`.

2.Implement `IPermissionGrantingDbContext`.

```c#
public class AppDbContext : DbContextBase
{
	public DbSet<GrantedPermission> GrantedPermissions { get;set; }
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.ConfigurePermissionGranting();
     }
}
```
3.Add permission-granting store.

```c#
	// ...configure services
	services.AddPermissionGrantingStore<AppDbContext >();
```