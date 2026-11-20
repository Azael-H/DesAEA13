using DesAEA13.Data;
using DesAEA13.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesAEA13.Controllers;

public class CoursesController : Controller
{
    private readonly AppDbContext _context;

    public CoursesController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var courses = await _context.Courses.Where(c => c.Active).ToListAsync();
        return View(courses);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await _context.Courses.FirstOrDefaultAsync(c => c.IdCourse == id && c.Active);
        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Credit")] Course course)
    {
        if (!ModelState.IsValid)
        {
            return View(course);
        }

        course.Active = true;
        _context.Add(course);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await _context.Courses.FirstOrDefaultAsync(c => c.IdCourse == id && c.Active);
        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdCourse,Name,Credit")] Course course)
    {
        if (id != course.IdCourse)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(course);
        }

        var existing = await _context.Courses.FirstOrDefaultAsync(c => c.IdCourse == id && c.Active);
        if (existing == null)
        {
            return NotFound();
        }

        existing.Name = course.Name;
        existing.Credit = course.Credit;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await _context.Courses.FirstOrDefaultAsync(c => c.IdCourse == id && c.Active);
        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course != null)
        {
            course.Active = false;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
