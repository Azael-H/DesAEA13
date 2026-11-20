using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesAEA13.Models;

public class Enrollment
{
    [Key]
    public int IdEnrollment { get; set; }

    public int IdStudent { get; set; }

    public int IdCourse { get; set; }

    public DateTime Date { get; set; }

    [ForeignKey(nameof(IdStudent))]
    public Student? Student { get; set; }

    [ForeignKey(nameof(IdCourse))]
    public Course? Course { get; set; }
}
