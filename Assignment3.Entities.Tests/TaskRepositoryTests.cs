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
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        context.Tasks.AddRange(new Task{Title = "Task1", Id = 1, State = State.New }, new Task{Title = "Task2", Id = 2, State = State.New });
        context.Tags.Add(new Tag{Name = "Tag1", Id = 1});
        context.SaveChanges();

        _context = context;
        _repository = new TaskRepository(_context);
    }

    [Fact]
    public void Create_given_Task_returns_Created_with_Task(){
        var (response, created) = _repository.Create(new TaskCreateDTO(Title: "TaskTitle", AssignedToId: 3, Description: "TagTest", Tags: new List<string>()));
        response.Should().Be(Response.Created);
        created.Should().Be(3);
    }

    [Fact]
    public void Delete_given_TaskId_returns_Deleted(){
        var response = _repository.Delete(2);
        response.Should().Be(Response.Deleted);
    }

    public void Dispose() {
        _context.Dispose();
    }
}
