using E_LearningV3;
using E_LearningV3.Models;
using E_LearningV3.Services;
using E_LearningV3.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace E_LearningV3.Controllers
{
    public class ExamFinalsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly CertificateService _certificateService;
        private readonly UserManager<User> _userManager;

        public ExamFinalsController(AppDbContext context, CertificateService certificateService, UserManager<User> userManager)
        {
            _context = context;
            _certificateService = certificateService;
            _userManager = userManager;
        }

        // GET: ExamFinals
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ExamFinales.Include(e => e.Course);
            return View(await appDbContext.ToListAsync());
        }

        // GET: ExamFinals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examFinal = await _context.ExamFinales
                .Include(e => e.Course)
                .FirstOrDefaultAsync(m => m.ExamFinalId == id);
            if (examFinal == null)
            {
                return NotFound();
            }

            return View(examFinal);
        }

        // GET: ExamFinals/Create
        public IActionResult Create(int? courseId)
        {
            ViewBag.SelectedCourseId = courseId;
            return View();
        }

        // POST: ExamFinals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExamFinalId,Title,QuestionsJson,AnswersJson,IsLocked,CourseId")] ExamFinal examFinal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(examFinal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit","Courses",new {id=examFinal.CourseId});
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseName", examFinal.CourseId);
            return View(examFinal);
        }

        // GET: ExamFinals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examFinal = await _context.ExamFinales.FindAsync(id);
            if (examFinal == null)
            {
                return NotFound();
            }
            ViewBag.SelectedCourseId = examFinal.CourseId;
            return View(examFinal);
        }

        // POST: ExamFinals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExamFinalId,Title,QuestionsJson,AnswersJson,IsLocked,CourseId")] ExamFinal examFinal)
        {
            if (id != examFinal.ExamFinalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(examFinal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamFinalExists(examFinal.ExamFinalId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "Courses", new { id = examFinal.CourseId });
            }
            ViewBag.SelectedCourseId = examFinal.CourseId;
            return View(examFinal);
        }

        // GET: ExamFinals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examFinal = await _context.ExamFinales
                .Include(e => e.Course)
                .FirstOrDefaultAsync(m => m.ExamFinalId == id);
            if (examFinal == null)
            {
                return NotFound();
            }

            return View(examFinal);
        }

        // POST: ExamFinals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var examFinal = await _context.ExamFinales.FindAsync(id);
            if (examFinal != null)
            {
                _context.ExamFinales.Remove(examFinal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("MyCourses", "Prof");
        }

        private bool ExamFinalExists(int id)
        {
            return _context.ExamFinales.Any(e => e.ExamFinalId == id);
        }

        // GET: ExamFinals/ToggleLock/5
        public async Task<IActionResult> ToggleLock(int id)
        {
            var exam = await _context.ExamFinales.FindAsync(id);
            if (exam == null)
                return NotFound();

            // Toggle the lock
            exam.IsLocked = !exam.IsLocked;

            _context.Update(exam);
            await _context.SaveChangesAsync();

            // Redirect back to the same page, e.g., ExamFinals Index
            return RedirectToAction("Edit", "Courses", new { id = exam.CourseId });
        }
        public async Task<IActionResult> TakeExam(int id)
        {
            var exam = await _context.ExamFinales
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.ExamFinalId == id);

            if (exam == null)
                return NotFound();

            // Optionally: check if the student already passed
            var studentIdClaim = User.FindFirst("StudentId")?.Value;
            if (string.IsNullOrEmpty(studentIdClaim)) return Forbid();
            int studentId = int.Parse(studentIdClaim);

            bool passed = _context.ExamScores
                .Any(s => s.ExamFinalId == exam.ExamFinalId
                        && s.StudentId == studentId
                        && s.ScoreValue >= 75);

            var questions = JsonSerializer.Deserialize<List<QuestionModel>>(exam.QuestionsJson) ?? new List<QuestionModel>();

            var vm = new FinalExamViewModel
            {
                ExamFinalId = exam.ExamFinalId,
                Title = exam.Title,
                ExamPassed = passed,
                Questions = questions
            };

            ViewBag.ExamPassed = passed;

            return View("/Views/Student/TakeExam.cshtml",vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitExam(FinalExamViewModel vm, string[] Answers)
        {
            if (vm == null || Answers == null)
                return BadRequest();

            var exam = await _context.ExamFinales
                .FirstOrDefaultAsync(e => e.ExamFinalId == vm.ExamFinalId);

            if (exam == null)
                return NotFound();

            var studentIdClaim = User.FindFirst("StudentId")?.Value;
            if (string.IsNullOrEmpty(studentIdClaim)) return Forbid();
            int studentId = int.Parse(studentIdClaim);

            // Deserialize correct answers
            var correctAnswers = JsonSerializer.Deserialize<List<string>>(exam.AnswersJson) ?? new List<string>();

            // Calculate score
            int score = 0;
            for (int i = 0; i < correctAnswers.Count && i < Answers.Length; i++)
            {
                if (string.Equals(correctAnswers[i], Answers[i], StringComparison.OrdinalIgnoreCase))
                    score += 100 / correctAnswers.Count;
            }

            // Save or update score
            var existingScore = await _context.ExamScores
                .FirstOrDefaultAsync(s => s.ExamFinalId == exam.ExamFinalId && s.StudentId == studentId);

            if (existingScore != null)
            {
                existingScore.ScoreValue = score;
                _context.Update(existingScore);
            }
            else
            {
                _context.Add(new ExamScore
                {
                    ExamFinalId = exam.ExamFinalId,
                    StudentId = studentId,
                    ScoreValue = score,
                    TakenAt = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();

            if (score >= 75)
            {
                // Passed: redirect to course or success page
                TempData["ExamResult"] = $"✅ You passed the exam with {score}%!";
                var alreadyHasCertificate = await _context.Certificates
                    .AnyAsync(c => c.StudentId == studentId && c.CourseId == exam.CourseId);
                if (!alreadyHasCertificate)
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    var student = await _context.Students.FindAsync(studentId);
                    var course = await _context.Courses.FindAsync(exam.CourseId);

                    var pdfBytes = _certificateService.GenerateCertificate(
                        currentUser.FullName,
                        course.CourseName,
                        DateTime.Now
                    );

                    var fileName = $"course_{course.CourseId}_student_{studentId}.pdf";
                    var filePath = Path.Combine("wwwroot/certificates", fileName);

                    System.IO.File.WriteAllBytes(filePath, pdfBytes);

                    _context.Certificates.Add(new Certificate
                    {
                        StudentId = studentId,
                        CourseId = course.CourseId,
                        FilePath = $"certificates/{fileName}"
                    });

                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Learn", "Student", new { id = exam.CourseId });
            }
            else
            {
                // Failed: show same exam page with message
                TempData["ExamResult"] = $"❌ You failed the exam with {score}%. Try again!";
                var questions = JsonSerializer.Deserialize<List<QuestionModel>>(exam.QuestionsJson) ?? new List<QuestionModel>();
                var vmFail = new FinalExamViewModel
                {
                    ExamFinalId = exam.ExamFinalId,
                    Title = exam.Title,
                    ExamPassed = false,
                    Questions = questions
                };
                return View("/Views/Student/TakeExam.cshtml", vmFail);
            }
        }



    }
}
