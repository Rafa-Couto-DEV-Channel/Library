using Library.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<BookModel> Book { get; set; }
    public DbSet<ClientModel> Client { get; set; }
    public DbSet<ReservationModel> Reservation { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken ct)
    {
        this.ApplyAudit();
        return base.SaveChangesAsync(ct);
    }

    private void ApplyAudit()
    {
 

        foreach (var entry in ChangeTracker.Entries())
        {
            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;
        
            if (entry.State == EntityState.Added)
            {
                SetIfExists(entry, "Id", id);
                SetIfExists(entry, "CreatedAt", now);
                SetIfExists(entry, "UpdatedAt", now);

            } else if(entry.State == EntityState.Modified)
            {
                SetIfExists(entry, "UpdatedAt", now);

            } else if(entry.State == EntityState.Deleted)
            {
                SetIfExists(entry, "UpdatedAt", now);
                SetIfExists(entry, "DeletedAt", now);
                                
            } 
        }

    }

    private static void SetIfExists(EntityEntry entry, string property, object value)
    {
        var getEntry = entry.Properties.FirstOrDefault(p => p.Metadata.Name == property);

        if (getEntry is { } prop)
        {
            prop.CurrentValue = value;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientModel>()
        .HasIndex(u  => u.Email)
        .IsUnique();
    }

}
