using DesAEA13.Data;
using DesAEA13.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesAEA13.Controllers;

public class GradesController : Controller
{
    private readonly AppDbContext _context;

    public GradesController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var grades = await _context.Grades.Where(g => g.Active).ToListAsync();
        return View(grades);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var grade = await _context.Grades.FirstOrDefaultAsync(g => g.IdGrade == id && g.Active);
        if (grade == null)
        {
            return NotFound();
        }

        return View(grade);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description")] Grade grade)
    {
        if (!ModelState.IsValid)
        {
            return View(grade);
        }

        grade.Active = true;
        _context.Add(grade);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var grade = await _context.Grades.FirstOrDefaultAsync(g => g.IdGrade == id && g.Active);
        if (grade == null)
        {
            return NotFound();
        }

        return View(grade);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdGrade,Name,Description")] Grade grade)
    {
        if (id != grade.IdGrade)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(grade);
        }

        var existing = await _context.Grades.FirstOrDefaultAsync(g => g.IdGrade == id && g.Active);
        if (existing == null)
        {
            return NotFound();
        }

        existing.Name = grade.Name;
        existing.Description = grade.Description;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var grade = await _context.Grades.FirstOrDefaultAsync(g => g.IdGrade == id && g.Active);
        if (grade == null)
        {
            return NotFound();
        }

        return View(grade);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var grade = await _context.Grades.FindAsync(id);
        if (grade != null)
        {
            grade.Active = false;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
