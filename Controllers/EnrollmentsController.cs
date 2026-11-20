using DesAEA13.Data;
using DesAEA13.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DesAEA13.Controllers;

public class EnrollmentsController : Controller
{
    private readonly AppDbContext _context;

    public EnrollmentsController(AppDbContext context)
    {
        _context = context;
    }

    private async Task LoadLists(int? student = null, int? course = null)
    {
        var students = await _context.Students
            .OrderBy(s => s.LastName)
            .Select(s => new { s.IdStudent, FullName = s.FirstName + " " + s.LastName })
            .ToListAsync();
        var courses = await _context.Courses.Where(c => c.Active).ToListAsync();
        ViewBag.IdStudent = new SelectList(students, "IdStudent", "FullName", student);
        ViewBag.IdCourse = new SelectList(courses, "IdCourse", "Name", course);
    }

    public async Task<IActionResult> Index()
    {
        var enrollments = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .OrderByDescending(e => e.Date)
            .ToListAsync();
        return View(enrollments);
    }

    public async Task<IActionResult> Create()
    {
        await LoadLists();
        return View(new Enrollment { Date = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdStudent,IdCourse,Date")] Enrollment enrollment)
    {
        if (!ModelState.IsValid)
        {
            await LoadLists(enrollment.IdStudent, enrollment.IdCourse);
            return View(enrollment);
        }

        _context.Add(enrollment);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var enrollment = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .FirstOrDefaultAsync(e => e.IdEnrollment == id);
        if (enrollment == null)
        {
            return NotFound();
        }

        return View(enrollment);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment != null)
        {
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
