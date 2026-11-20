using DesAEA13.Data;
using DesAEA13.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DesAEA13.Controllers;

public class StudentsController : Controller
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    private async Task LoadGrades(int? selected = null)
    {
        var grades = await _context.Grades.Where(g => g.Active).ToListAsync();
        ViewBag.IdGrade = new SelectList(grades, "IdGrade", "Name", selected);
    }

    public async Task<IActionResult> Index()
    {
        var students = await _context.Students
            .Include(s => s.Grade)
            .OrderBy(s => s.LastName)
            .ToListAsync();
        return View(students);
    }

    public async Task<IActionResult> Create()
    {
        await LoadGrades();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdGrade,FirstName,LastName,Phone,Email")] Student student)
    {
        if (!ModelState.IsValid)
        {
            await LoadGrades(student.IdGrade);
            return View(student);
        }

        _context.Add(student);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        await LoadGrades(student.IdGrade);
        return View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdStudent,IdGrade,FirstName,LastName,Phone,Email")] Student student)
    {
        if (id != student.IdStudent)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await LoadGrades(student.IdGrade);
            return View(student);
        }

        var existing = await _context.Students.FindAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        existing.IdGrade = student.IdGrade;
        existing.FirstName = student.FirstName;
        existing.LastName = student.LastName;
        existing.Phone = student.Phone;
        existing.Email = student.Email;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Students
            .Include(s => s.Grade)
            .FirstOrDefaultAsync(s => s.IdStudent == id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return RedirectToAction(nameof(Index));
        }

        if (await _context.Enrollments.AnyAsync(e => e.IdStudent == id))
        {
            ModelState.AddModelError(string.Empty, "El estudiante tiene matrículas y no se puede eliminar.");
            var current = await _context.Students.Include(s => s.Grade).FirstAsync(s => s.IdStudent == id);
            return View("Delete", current);
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
