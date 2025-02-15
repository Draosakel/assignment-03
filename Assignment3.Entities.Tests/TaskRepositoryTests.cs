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

    public TaskRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        //context.Tasks.Include(t => t.Tags).ToList();
        //context.Tags.Include(t => t.Tasks).ToList();
        context.Database.EnsureCreated();
        var task1 = new Task { Title = "Task1", Id = 1, State = State.New, AssignedToId = 1 };
        var task2 = new Task { Title = "Task2", Id = 2, State = State.New, AssignedToId = 1 };
        var task4 = new Task { Title = "Task4", Id = 4, State = State.Removed };
        var tag1 = new Tag { Name = "Tag1", Id = 1 };
        context.Tags.Add(tag1);
        var tagTask = new Task { Title = "TagTask", Id = 9, State = State.New, Tags = new[] { new Tag { Name = "TagTag" } } };
        context.Tasks.AddRange(task1, task2, tagTask, task4);
        var user = new User { Email = "", Name = "UserName", Id = 1 };
        context.Users.Add(user);
        context.SaveChanges();

        _context = context;
        _repository = new TaskRepository(_context);
    }

    [Fact]
    public void Create_given_Task_returns_Created_with_Task()
    {
        var (response, created) = _repository.Create(new TaskCreateDTO(Title: "TaskTitle", AssignedToId: 3, Description: "TagTest", Tags: new List<string>()));
        response.Should().Be(Response.Created);
        created.Should().Be(3);
    }

    [Fact]
    public void Delete_given_TaskId_returns_Deleted()
    {
        var response = _repository.Delete(2);
        response.Should().Be(Response.Deleted);
    }

    [Fact]
    public void Read_given_TaskId_Returns_TaskDetailsDTO()
    {
        TaskDetailsDTO response = _repository.Read(1);
        response.Should().Be(new TaskDetailsDTO(1, "Task1", null, DateTime.Now.Date, "UserName", null, State.New, DateTime.MinValue));
    }

    [Fact]
    public void ReadAll_Returns_IReadOnlyCollection()
    {
        var response = _repository.ReadAll();
        response.Should().BeEquivalentTo(new List<TaskDTO>() { new TaskDTO(1, "Task1", "UserName", null, State.New),
            new TaskDTO(2, "Task2", "UserName", null, State.New),
            new TaskDTO(9, "TagTask", null, new []{ "TagTag" }, State.New),
            new TaskDTO(4, "Task4", null, null, State.Removed)});
    }

    [Fact]
    public void ReadAllByState_given_State_New_Returns_IReadOnlyCollection()
    {
        var response = _repository.ReadAllByState(State.New);
        response.Should().BeEquivalentTo(new List<TaskDTO>() { new TaskDTO(1, "Task1", "UserName", null, State.New),
            new TaskDTO(2, "Task2", "UserName", null, State.New),
            new TaskDTO(9, "TagTask", null, new []{ "TagTag" }, State.New) });
    }

    [Fact]
    public void ReadAllByUser_given_user_with_tasks_returns_tasks()
    {
        var response = _repository.ReadAllByUser(1);
        response.Should().HaveCount(2);
    }

    [Fact]
    public void ReadAllByTag()
    {
        //var dto = new TaskUpdateDTO(9, "TagTask", null, null, new[] { "TagTag" }, State.New);
        //_repository.Update(dto);

        var response = _repository.ReadAllByTag("TagTag");
        response.Should().BeEquivalentTo(new[] { new TaskDTO(9, "TagTask", null, new[] { "TagTag" }, State.New) });
    }

    [Fact]
    public void ReadAllRemoved_Returns_IReadOnlyCollection_State_Removed()
    {
        var response = _repository.ReadAllRemoved();
        response.Should().BeEquivalentTo(new List<TaskDTO>() { new TaskDTO(4, "Task4", null, null, State.Removed) });
    }

    [Fact]
    public void Update_given_TaskUpdateDTO_Returns_State_Updated()
    {
        var updateTaskDTO = new TaskUpdateDTO(2, "UpdatedTask2", null, null, new List<string>(), State.Active);
        var response = _repository.Update(updateTaskDTO);
        response.Should().Be(Response.Updated);
    }

    [Fact]
    public void Update_given_NOT_Existing_TaskUpdateDTO_Returns_State_Updated()
    {
        var updateTaskDTO = new TaskUpdateDTO(5, "UpdatedTask2", null, null, new List<string>(), State.Active);
        var response = _repository.Update(updateTaskDTO);
        response.Should().Be(Response.NotFound);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
