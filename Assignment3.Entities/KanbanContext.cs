using Assignment3.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Assignment3.Entities;

public class KanbanContext : DbContext
{
    public KanbanContext(DbContextOptions<KanbanContext> options) : base(options)
    {

    }

    public virtual DbSet<Task> Tasks => Set<Task>();
    public virtual DbSet<Tag> Tags => Set<Tag>();
    public virtual DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>(entity => entity.Property(e => e.State).HasConversion(new EnumToStringConverter<State>()));
        modelBuilder.Entity<User>(entity => entity.HasIndex(e => e.Email).IsUnique());
        modelBuilder.Entity<Tag>(entity => entity.HasIndex(e => e.Name).IsUnique());
    }
}
