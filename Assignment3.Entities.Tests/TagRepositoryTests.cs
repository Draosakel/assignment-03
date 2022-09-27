namespace Assignment3.Entities.Tests;
using Assignment3.Entities;
using Assignment3.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Xunit;

public sealed class TagRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly TagRepositoryTests _repository;

    public TagRepositoryTests() {
        var connection = new SqliteConnection("Filename=:memory");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        //context.Cities.AddRange(new City("Metropolis") { Id = 1 }, new City("Gotham City") { Id = 2 });
        context.Tags.Add(new Tag { Id = 1, Name = "TagTest", Tasks = new List<Task>() });
        context.SaveChanges();

        _context = context;
        //_repository = new TagRepositoryTests(_context);
    }
    //var connection = new SqliteConnection("Filename=:memory:")
    [Fact]
    public void CreateTaskTest(){
       // var (response, created) = _repository.Create(new TagCreateDTO("TagTest3"));
    }

    public void Dispose() {
        _context.Dispose();
    }
}
