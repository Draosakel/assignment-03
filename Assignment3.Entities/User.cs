using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Assignment3.Entities;

public class User
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }

    [Required, StringLength(100)]
    public string Email { get; set; }
    [NotMapped]
    public List<Task> Tasks { get; set; }
}
