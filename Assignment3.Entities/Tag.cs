using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Assignment3.Entities;

public class Tag
{
    public int Id{ get; set; }

    [Required, StringLength(50)]
    public string Name{ get; set; }
    [NotMapped]
    public ICollection<Task> Tasks { get; set; }
}
