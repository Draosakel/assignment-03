using Assignment3.Core;
using System.Threading.Tasks;

namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{
    KanbanContext _context;
    public TaskRepository(KanbanContext _context){
        this._context = _context;
    }


    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        var tags = task.Tags != null ? _context.Tags.ToList().Where(tag => task.Tags.Contains(tag.Name)).ToList() : null;
        var t = new Task() { Title = task.Title, Id = task.AssignedToId, Description = task.Description, Tags = tags, State = State.New };
        _context.Tasks.Add(t);
        return (Response.Created, (int)t.Id);
    }

    public Response Delete(int taskId)
    {
        var t = _context.Tasks.Where(a => a.Id == taskId).FirstOrDefault();

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
                _context.Tasks.Remove(t);
                return Response.Deleted;
            default:
                return Response.BadRequest;
        }

    }

    public TaskDetailsDTO Read(int taskId)
    {
        var t = _context.Tasks.Where(a => a.Id == taskId).FirstOrDefault();

        if (t == null) return null;

        var tags = (IReadOnlyCollection<string>)t.Tags;
        var task = new TaskDetailsDTO((int)t.Id, t.Title, t.Description, DateTime.Now.Date, t.AssignedTo?.Name, tags, t.State, DateTime.MinValue);
        return task;
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        var tempList = new List<TaskDTO>();
        foreach (Task t in _context.Tasks)
        {
            var tags = t.Tags?.Select(a => a.Name).ToList();
            tempList.Add(new TaskDTO((int)t.Id, t.Title, t.AssignedTo?.Name, tags, t.State));
        }
        return tempList.Count > 0 ? tempList : null;
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
        return ReadAll()?.Where(a => a.State == state).ToList();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        return ReadAll()?.Where(a => a.Tags != null && a.Tags.Contains(tag)).ToList();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        var userName = _context.Users.Where(a => a.Id == userId).Select(a => a.Name).FirstOrDefault();
        return ReadAll()?.Where(a => a.AssignedToName == userName).ToList();

    }

    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        return ReadAll()?.Where(a => a.State == State.Removed).ToList();

    }

    public Response Update(TaskUpdateDTO task)
    {
        var t = _context.Tasks.FirstOrDefault(a => a.Id == task.Id);

        bool exists = t != null;
        if (!exists) return Response.NotFound;

        t.Title = task.Title;
        t.Description = task.Description;
        var tags = task.Tags != null ? _context.Tags.ToList().Where(tag => task.Tags.Contains(tag.Name)).ToList() : null;
        t.Tags = tags;
        t.AssignedToId = task.AssignedToId;
        t.State = task.State;

        return Response.Updated;
    }
}
