using E_LearningV3;
using E_LearningV3.Models;
using E_LearningV3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_LearningV3.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ChapterService _chapterService;

        public CoursesController(AppDbContext context, ChapterService chapterService)
        {
            _context = context;
            _chapterService = chapterService;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Courses.Include(c => c.Professor);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id, string? returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Professor).ThenInclude(p => p.User)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["ProfessorId"] = new SelectList(_context.Professors, "ProfessorId", "UserId");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseName,Description")] Course course)
        {
            if (!ModelState.IsValid)
                return View(course);
            var professorIdClaim = User.FindFirst("ProfessorId")?.Value;
            if (string.IsNullOrEmpty(professorIdClaim))
            {
                return BadRequest("Logged-in user is not a professor or claim missing.");
            }
            course.ProfessorId = int.Parse(professorIdClaim);
            _context.Add(course);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyCourses", "Prof");
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            int professorId = int.Parse(User.FindFirst("ProfessorId")!.Value);

            var course = await _context.Courses
                .Include(c => c.ExamFinal)   // ✅ IMPORTANT
                .AsNoTracking()
                .FirstOrDefaultAsync(c =>
                c.CourseId == id &&
                c.ProfessorId == professorId);
            if (course == null)
                return NotFound(); // or Forbid()
            course.Chapters = await _chapterService.GetChaptersByCourseId(course.CourseId);
            //ViewData["ProfessorId"] = new SelectList(_context.Professors, "ProfessorId", "UserId", course.ProfessorId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseName,Description")] Course course)
        {
            int professorId = int.Parse(User.FindFirst("ProfessorId")!.Value);
            if (id != course.CourseId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(course);
            var existingCourse = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == id &&c.ProfessorId == professorId);

            if (existingCourse == null)
                return Forbid();
            existingCourse.CourseName = course.CourseName;
            existingCourse.Description = course.Description;
            await _context.SaveChangesAsync();

            return RedirectToAction("MyCourses", "Prof");

            // ViewData["ProfessorId"] = new SelectList(_context.Professors, "ProfessorId", "UserId", course.ProfessorId);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int professorId = int.Parse(User.FindFirst("ProfessorId")!.Value);
            var course = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.CourseId == id && c.ProfessorId == professorId);

            if (course == null)
                return NotFound(); // or Forbid() if you prefer

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int professorId = int.Parse(User.FindFirst("ProfessorId")!.Value);

            // Only allow deleting courses owned by the logged-in professor
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == id && c.ProfessorId == professorId);

            if (course == null)
                return Forbid();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyCourses", "Prof");
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
