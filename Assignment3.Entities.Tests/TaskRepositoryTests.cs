namespace Assignment3.Entities.Tests;
using Assignment3.Entities;
using Assignment3.Core;
using Microsoft.Data.SqlClient;
using Xunit;

public class TaskRepositoryTests
{
    //var connection = new SqliteConnection("Filename=:memory:")
    [Fact]
    public void CreateTaskTest(){
        TaskCreateDTO t = new TaskCreateDTO(Title: "Task", AssignedToId: 1, Description: "test task", Tags: null);
        KanbanContext _context = new KanbanContext(options);
        TaskRepository taskRepository = new TaskRepository(_context);
        var result = taskRepository.Create(t);
        Assert.Equal(result.Response, Response.Created);
    }
}
