namespace Assignment3.Entities.Tests;
using Assignment3.Entities;
using Assignment3.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Xunit;

public sealed class TaskRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly TaskRepository _repository;

    public TaskRepositoryTests() {
        var connection = new SqliteConnection("Filename=:memory");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        //context.Cities.AddRange(new City("Metropolis") { Id = 1 }, new City("Gotham City") { Id = 2 });
        context.Tasks.Add(new Task { AssignedToId = 1, Title = "Task", Tags = new List<Tag>(), State = State.New });
        context.SaveChanges();

        _context = context;
        _repository = new TaskRepository(_context);
    }
    //var connection = new SqliteConnection("Filename=:memory:")
    [Fact]
    public void CreateTaskTest(){
        var (response, created) = _repository.Create(new TaskCreateDTO(Title: "TaskTitle", AssignedToId: 2, Description: "TagTest", Tags: new List<String>()));
        response.Should().Be(Response.Created);
    }

    public void Dispose() {
        _context.Dispose();
    }
}
