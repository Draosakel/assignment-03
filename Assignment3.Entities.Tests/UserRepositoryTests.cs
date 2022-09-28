namespace Assignment3.Entities.Tests;
using Assignment3.Entities;
using Assignment3.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Xunit;

public sealed class UserRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests() {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        context.Users.Add(new User{ Email = "something@", Name = "Bob", Id = 1 });
        context.Users.Add(new User{ Email = "anotherthing@", Name = "Frederick", Id = 2 });
        context.SaveChanges();

        _context = context;
        _repository = new UserRepository(_context);
    }

    [Fact]
    public void 

    public void Dispose() {
        _context.Dispose();
    }
}
