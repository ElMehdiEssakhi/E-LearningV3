using E_LearningV3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_LearningV3.Controllers
{
    [Authorize(Roles = "Student")]
    public class EnrollmentsController : Controller
    {
        private readonly AppDbContext _context;
        public EnrollmentsController(AppDbContext context)
        {
            _context = context;
        }
        // GET: EnrollmentsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: EnrollmentsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int courseId)
        {
            // Get current student id from claims
            var claim = User.FindFirst("StudentId")?.Value;
            if (string.IsNullOrEmpty(claim))
            {
                return Forbid(); // or redirect to login
            }

            int studentId = int.Parse(claim);

                var enrollment = new Enrollment
                {
                    StudentId = studentId,
                    CourseId = courseId
                };

                _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            Console.WriteLine($"_________enrolled: {enrollment.StudentId}");

            // Redirect back to Browse or Course Details
            return RedirectToAction("Learn", "Student", new { id = courseId });
        }

        // GET: Enrollments/MyEnrollments
        public async Task<IActionResult> MyEnrollments()
        {
            // Get current student id from claims
            var studentClaim = User.FindFirst("StudentId")?.Value;
            if (string.IsNullOrEmpty(studentClaim))
                return Forbid(); // or redirect to login

            int studentId = int.Parse(studentClaim);

            // Get all courses this student is enrolled in
            var courses = await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                .ThenInclude(c => c.Professor).ThenInclude(p => p.User)
                .Select(e => e.Course)
                .ToListAsync();

            return View("~/Views/Student/MyEnrollments.cshtml",courses);
        }


        // GET: EnrollmentsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EnrollmentsController/Edit/5
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

        // GET: EnrollmentsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EnrollmentsController/Delete/5
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
