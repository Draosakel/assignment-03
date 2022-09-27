using Microsoft.EntityFrameworkCore;

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
        modelBuilder.Entity<Task>()
                .HasMany<Tag>(t => t.Tags)
                .WithMany(c => c.Tasks);
    }
}
