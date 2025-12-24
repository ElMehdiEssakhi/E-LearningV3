using E_LearningV3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_LearningV3.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        public StudentController(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Welcome()
        {
            // 1. Get the current user
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser != null)
            {
                // 2. Access the custom property
                ViewData["DisplayName"] = currentUser.FullName;
            }
            else
            {
                ViewData["DisplayName"] = "Professor";
            }

            // 2. Get StudentId from claims
            var studentClaim = User.FindFirst("StudentId")?.Value;
            if (string.IsNullOrEmpty(studentClaim))
                return Forbid();

            int studentId = int.Parse(studentClaim);

            // 3. Get enrolled courses
            var courses = await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                .Select(e => e.Course)
                .ToListAsync();

            ViewData["Courses"] = courses;

            return View();
        }
        public async Task<IActionResult> Browse(string? search)
        {
            var coursesQuery = _context.Courses
                .Include(c => c.Chapters)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                coursesQuery = coursesQuery.Where(c =>
                    c.CourseName.Contains(search) ||
                    c.Description.Contains(search));
            }
            var studentClaim = User.FindFirst("StudentId")?.Value;
            var enrolledIds = new HashSet<int>();

            if (!string.IsNullOrEmpty(studentClaim))
            {
                int studentId = int.Parse(studentClaim);

                // Get only the IDs of courses the student is already in
                var idList = await _context.Enrollments
                    .Where(e => e.StudentId == studentId)
                    .Select(e => e.CourseId)
                    .ToListAsync();

                enrolledIds = new HashSet<int>(idList);
            }
            ViewBag.EnrolledCourseIds = enrolledIds;
            var courses = await coursesQuery.ToListAsync();

            ViewData["Search"] = search;

            return View(courses);
        }
        public async Task<IActionResult> Learn(int id, int? chapterId, int? examId)
        {
            var course = await _context.Courses
                .Include(c => c.Chapters)
                    .ThenInclude(ch => ch.Contents)
                .Include(c => c.Chapters)
                    .ThenInclude(ch => ch.Quizzes)
                .Include(c => c.ExamFinal)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
                return NotFound();

            Chapter selectedChapter = null;
            ExamFinal selectedExam = null;

            if (chapterId != null)
            {
                selectedChapter = course.Chapters.FirstOrDefault(c => c.ChapId == chapterId);
            }
            else if (examId != null && course.ExamFinal != null)
            {
                selectedExam = course.ExamFinal;
            }
            ViewBag.SelectedChapter = selectedChapter;
            ViewBag.SelectedExam = selectedExam;

            var studentIdClaim = User.FindFirst("StudentId")?.Value;
            if (string.IsNullOrEmpty(studentIdClaim)) return Forbid();
            int studentId = int.Parse(studentIdClaim);

            bool quizPassed = selectedChapter?.Quizzes.Any(q =>
            _context.Scores.Any(s => s.QuizId == q.QuizId && s.StudentId == studentId && s.ScoreValue >= 50)
            ) ?? false;

            ViewBag.QuizPassed = quizPassed;

            bool finalExamPassed = false;
            if (course.ExamFinal != null)
            {
                finalExamPassed = _context.ExamScores
                    .Any(s => s.ExamFinalId == course.ExamFinal.ExamFinalId
                            && s.StudentId == studentId
                            && s.ScoreValue >= 75);
            }

            ViewBag.FinalExam = course.ExamFinal;
            ViewBag.FinalExamPassed = finalExamPassed;
            ViewBag.FinalExamSelected = selectedExam != null;
            if (finalExamPassed)
            {
                var certificate = await _context.Certificates
                    .FirstOrDefaultAsync(c =>
                        c.StudentId == studentId &&
                        c.CourseId == course.CourseId);

                ViewBag.CertificateId = certificate?.CertificateId;
            }

            return View(course);

        }

        [HttpPost]
        public async Task<IActionResult> CompleteChapter(int courseId, int currentChapterId)
        {
            var studentIdClaim = User.FindFirst("StudentId")?.Value;
            if (string.IsNullOrEmpty(studentIdClaim)) return Forbid();
            int studentId = int.Parse(studentIdClaim);

            // 1. Get the list of Quiz IDs belonging to this chapter
            var quizIds = await _context.Quizzes
                .Where(q => q.ChapId == currentChapterId)
                .Select(q => q.QuizId)
                .ToListAsync();

            // 2. If the chapter has quizzes, check if student passed them
            if (quizIds.Any())
            {
                // Check if student has at least one passing score (e.g., >= 50) for EVERY quiz in the chapter
                foreach (var qId in quizIds)
                {
                    var hasPassed = await _context.Scores
                        .AnyAsync(s => s.StudentId == studentId && s.QuizId == qId && s.ScoreValue >= 50);

                    if (!hasPassed)
                    {
                        TempData["Error"] = "You must pass all chapter quizzes before continuing!";
                        return RedirectToAction("Learn", "Student", new { id = courseId, chapterId = currentChapterId });
                    }
                }
            }

            // 3. Find the Next Chapter based on ID (assuming sequential IDs)
            var nextChapter = await _context.Chapters
                .Where(c => c.CourseId == courseId && c.ChapId > currentChapterId)
                .OrderBy(c => c.ChapId)
                .FirstOrDefaultAsync();

            if (nextChapter != null)
            {
                return RedirectToAction("Learn", "Student", new { id = courseId, chapterId = nextChapter.ChapId });
            }

            // Finished course logic
            TempData["Success"] = "Course Completed!";
            return RedirectToAction("MyEnrollments", "Student");
        }
    }
}
