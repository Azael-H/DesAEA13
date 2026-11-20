using System.ComponentModel.DataAnnotations;

namespace DesAEA13.Models;

public class Course
{
    [Key]
    public int IdCourse { get; set; }

    [Required]
    [StringLength(45)]
    public string Name { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Credit { get; set; }

    public bool Active { get; set; } = true;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
