using E_LearningV3.Models;
using E_LearningV3.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace E_LearningV3.Controllers
{
    public class QuizController : Controller
    {

        private readonly AppDbContext _context;

        public QuizController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Take(int chapterId)
        {
            var quiz = _context.Quizzes
                .Include(q => q.Chapter)
                .FirstOrDefault(q => q.ChapId == chapterId);
            if (quiz == null) return NotFound();

            var questions = JsonSerializer.Deserialize<List<QuizQuestionVM>>(quiz.QuestionsJson)!;

            var vm = new QuizTakeVM
            {
                QuizId = quiz.QuizId,
                ChapterId = chapterId,
                CourseId = quiz.Chapter.CourseId,
                Questions = questions
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Submit(QuizTakeVM model)
        {
            var quiz = _context.Quizzes.First(q => q.QuizId == model.QuizId);

            var correctAnswers =
                JsonSerializer.Deserialize<Dictionary<int, string>>(quiz.AnswersJson)!;

            int correctCount = 0;

            foreach (var answer in model.UserAnswers)
            {
                if (correctAnswers.ContainsKey(answer.Key) &&
                    correctAnswers[answer.Key] == answer.Value)
                {
                    correctCount++;
                }
            }

            int totalQuestions = correctAnswers.Count;
            int scoreValue = (int)((double)correctCount / totalQuestions * 100);

            // ❌ FAIL → REPEAT QUIZ
            if (scoreValue < 50)
            {
                TempData["QuizError"] =
                    $"You scored {scoreValue}%. Minimum required is 50%. Try again.";

                return RedirectToAction("Take", new { chapterId = model.ChapterId });
            }

            var studentIdClaim = User.FindFirst("StudentId")?.Value;
            if (string.IsNullOrEmpty(studentIdClaim)) return Forbid();
            int studentId = int.Parse(studentIdClaim);

            _context.Scores.Add(new Score
            {
                QuizId = quiz.QuizId,
                StudentId = studentId,
                ScoreValue = scoreValue
            });

            _context.SaveChanges();
            TempData["QuizError"] = null;
            // Redirect to next chapter
            var nextChapter = _context.Chapters
                .Where(c => c.CourseId == model.CourseId && c.ChapId > model.ChapterId)
                .OrderBy(c => c.ChapId)
                .FirstOrDefault();

            if (nextChapter == null)
                return RedirectToAction("Completed", "Course", new { id = model.CourseId });

            return RedirectToAction(
                "Learn",
                "Student",
                new { id = nextChapter.CourseId, chapterId = nextChapter.ChapId }
            );
        }




        // GET: QuizController
        public ActionResult Index()
        {
            return View();
        }

        // GET: QuizController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: QuizController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuizController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QuizController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: QuizController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QuizController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: QuizController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
