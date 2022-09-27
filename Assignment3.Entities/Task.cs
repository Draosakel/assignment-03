using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Assignment3.Entities;

public class Task
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; }

    public User? AssignedTo { get; set; }

    public string? Description { get; set; }

    [Required]
    public State State { get; set; }
    public virtual ICollection<Tag> Tags { get; set; }

}

public enum State
{
    New,
    Active,
    Resolved,
    Closed,
    Removed
}

