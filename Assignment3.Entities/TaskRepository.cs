using Assignment3.Core;
using System.Threading.Tasks;

namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{
    private readonly List<Task> _task = new List<Task>();
    public IReadOnlyCollection<Task> Tasks => _task;
    KanbanContext _context;
    public TaskRepository(KanbanContext _context){
        this._context = _context;
    }


    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        var t = new Task() { Title = task.Title, Id = task.AssignedToId, Description = task.Description, Tags = (ICollection<Tag>)task.Tags, State = State.New };
        _context.Tasks.Add(t);
        return (Response.Created, (int)t.Id);
    }

    public Response Delete(int taskId)
    {
        var t = Tasks.FirstOrDefault(a => a.Id == taskId);

        bool exists = t != null;

        if (!exists) return Response.NotFound;
        switch (t.State)
        {
            case State.Resolved:
            case State.Closed:
            case State.Removed:
                return Response.Conflict;
            case State.Active:
                t.State = State.Removed;
                return Response.Deleted;
            case State.New:
                _task.Remove(t);
                return Response.Deleted;
            default:
                return Response.BadRequest;
        }

    }

    public TaskDetailsDTO Read(int taskId)
    {
        var t = Tasks.FirstOrDefault(a => a.Id == taskId);

        if (t == null) return null;

        return new TaskDetailsDTO((int)t.Id, t.Title, t.Description, DateTime.Now, t.AssignedTo.Name, (IReadOnlyCollection<string>)t.Tags, t.State, DateTime.MinValue);
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        var tempList = new List<TaskDTO>();
        foreach (Task t in Tasks)
        {
            tempList.Add(new TaskDTO((int)t.Id, t.Title, t.AssignedTo.Name, (IReadOnlyCollection<string>)t.Tags, t.State));
        }
        return tempList.Count > 0 ? tempList : null;
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
        var tempList = new List<TaskDTO>();
        foreach (Task t in Tasks.Where(a => a.State == state))
        {
            tempList.Add(new TaskDTO((int)t.Id, t.Title, t.AssignedTo.Name, (IReadOnlyCollection<string>)t.Tags, t.State));
        }
        return tempList.Count > 0 ? tempList : null;
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        var tempList = new List<TaskDTO>();
        foreach (Task t in Tasks.Where(a => ((IReadOnlyCollection<string>)a.Tags).Contains(tag)))
        {
            tempList.Add(new TaskDTO((int)t.Id, t.Title, t.AssignedTo.Name, (IReadOnlyCollection<string>)t.Tags, t.State));
        }
        return tempList.Count > 0 ? tempList : null;
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        var tempList = new List<TaskDTO>();
        foreach (Task t in Tasks.Where(a => a.AssignedTo.Id == userId))
        {
            tempList.Add(new TaskDTO((int)t.Id, t.Title, t.AssignedTo.Name, (IReadOnlyCollection<string>)t.Tags, t.State));
        }
        return tempList.Count > 0 ? tempList : null;
    }

    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        var tempList = new List<TaskDTO>();
        foreach (Task t in Tasks.Where(a => a.State == State.Removed))
        {
            tempList.Add(new TaskDTO((int)t.Id, t.Title, t.AssignedTo.Name, (IReadOnlyCollection<string>)t.Tags, t.State));
        }
        return tempList.Count > 0 ? tempList : null;
    }

    public Response Update(TaskUpdateDTO task)
    {
        var t = Tasks.FirstOrDefault(a => a.Id == task.Id);

        bool exists = t != null;
        if (!exists) return Response.NotFound;

        t.Title = task.Title;
        t.Description = task.Description;
        t.Tags = (ICollection<Tag>)task.Tags;
        t.AssignedToId = task.AssignedToId;
        t.State = task.State;

        return Response.Updated;
    }
}
