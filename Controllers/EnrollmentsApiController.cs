using DesAEA13.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesAEA13.Controllers;

[ApiController]
[Route("api/enrollments")]
public class EnrollmentsApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public EnrollmentsApiController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("by-course")]
    public async Task<IActionResult> ByCourse(string? course)
    {
        var query = _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(course))
        {
            query = query.Where(e => e.Course != null && e.Course.Name.Contains(course));
        }

        var enrollments = await query
            .OrderBy(e => e.Course!.Name)
            .ThenBy(e => e.Student!.LastName)
            .ToListAsync();

        return Ok(enrollments);
    }

    [HttpGet("by-grade")]
    public async Task<IActionResult> ByGrade(string? grade)
    {
        var query = _context.Enrollments
            .Include(e => e.Student)
                .ThenInclude(s => s!.Grade)
            .Include(e => e.Course)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(grade))
        {
            query = query.Where(e => e.Student != null && e.Student.Grade != null && e.Student.Grade.Name.Contains(grade));
        }

        var enrollments = await query
            .OrderBy(e => e.Course!.Name)
            .ThenBy(e => e.Student!.LastName)
            .ToListAsync();

        return Ok(enrollments);
    }
}
