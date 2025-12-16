using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_LearningV3;
using E_LearningV3.Models;
using E_LearningV3.Services;

namespace E_LearningV3.Controllers
{
    public class ChaptersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ContentService _contentService;
        private readonly QuizService _quizService;

        public ChaptersController(AppDbContext context,ContentService contentService,QuizService quizService)
        {
            _context = context;
            _contentService = contentService;
            _quizService = quizService;
        }

        // GET: Chapters
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Chapters.Include(c => c.Course);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Chapters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.ChapId == id);
            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }

        // GET: Chapters/Create
        public IActionResult Create(int courseId)
        {
            var chapter = new Chapter
            {
                CourseId = courseId
            };

            return View(chapter);
        }

        // POST: Chapters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChapId,Title,CourseId")] Chapter chapter)
        {
            int professorId = int.Parse(User.FindFirst("ProfessorId")!.Value);
            var course = await _context.Courses
        .FirstOrDefaultAsync(c => c.CourseId == chapter.CourseId && c.ProfessorId == professorId);

            if (course == null)
            {
                return Forbid(); // professor trying to add chapter to another's course
            }

            if (ModelState.IsValid)
            {
                _context.Add(chapter);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "Courses", new { id = chapter.CourseId }); // back to course edit page
            }

            return View(chapter);
        }

        // GET: Chapters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null)
            {
                return NotFound();
            }
            chapter.Contents = await _contentService.GetContentsByChapterId(chapter.ChapId);
            chapter.Quizzes = await _quizService.GetQuizzesByChapterId(chapter.ChapId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseName", chapter.CourseId);
            return View(chapter);
        }

        // POST: Chapters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChapId,Title,CourseId")] Chapter chapter)
        {
            if (id != chapter.ChapId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chapter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChapterExists(chapter.ChapId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseName", chapter.CourseId);
            return View(chapter);
        }

        // GET: Chapters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.ChapId == id);
            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }

        // POST: Chapters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter != null)
            {
                _context.Chapters.Remove(chapter);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChapterExists(int id)
        {
            return _context.Chapters.Any(e => e.ChapId == id);
        }
    }
}
