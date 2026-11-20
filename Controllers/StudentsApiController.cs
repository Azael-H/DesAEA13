using DesAEA13.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesAEA13.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsApiController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string? firstName, string? lastName, string? email)
    {
        var query = _context.Students.AsQueryable();

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            query = query.Where(s => s.FirstName.Contains(firstName));
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            query = query.Where(s => s.LastName.Contains(lastName));
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            query = query.Where(s => s.Email != null && s.Email.Contains(email));
        }

        var students = await query.OrderByDescending(s => s.LastName).ToListAsync();
        return Ok(students);
    }

    [HttpGet("search-by-grade")]
    public async Task<IActionResult> SearchByGrade(string? firstName, string? grade)
    {
        var query = _context.Students
            .Include(s => s.Grade)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            query = query.Where(s => s.FirstName.Contains(firstName));
        }

        if (!string.IsNullOrWhiteSpace(grade))
        {
            query = query.Where(s => s.Grade != null && s.Grade.Name.Contains(grade));
        }

        var students = await query
            .OrderByDescending(s => s.Grade!.Name)
            .ToListAsync();

        return Ok(students);
    }
}
