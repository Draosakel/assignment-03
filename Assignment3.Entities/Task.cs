using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities;

public class Task
{
    public DbSet<Tag> Tags { get; set; }
    public int Id { get; set; }
    public string Title
    {
        get
        {
            return Title;
        }
        set
        {
            if (value.Length > 100)
            {
                Title = "noTitle";
            }
            else Title = value;

        }
    }
    public User AssignedTo { get; set; }
    public string Description { get; set; }
    public enum State
    {
        New,
        Active,
        Resolved,
        Closed,
        Removed
    }
    public Task(string title, State state)
    {
        this.Title = title;
    }

}

