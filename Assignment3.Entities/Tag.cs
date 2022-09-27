using System.ComponentModel.DataAnnotations;

namespace Assignment3.Entities;

public class Tag
{
    public int Id{ get; set; }

    [Required, StringLength(50)]
    public string Name{ get; set; }

    public virtual ICollection<Task> Tasks { get; set; }
}
