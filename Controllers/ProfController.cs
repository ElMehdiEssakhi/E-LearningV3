using E_LearningV3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_LearningV3.Controllers
{
    [Authorize(Roles = "Prof")]
    public class ProfController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        public ProfController(UserManager<User> userManager,AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Welcome()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userManager.GetUserAsync(User);

            var latestEnrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .ThenInclude(s => s.User)
                .Where(e => e.Course.Professor.UserId == userId)
                .OrderByDescending(e => e.EnrollmentDate) 
                .AsNoTracking()
                .FirstOrDefaultAsync();

            ViewData["DisplayName"] = currentUser?.FullName ?? "Professor";

            // Pass the enrollment to the view via ViewData or a ViewModel
            ViewData["LatestEnrollment"] = latestEnrollment;

            return View();
        }

        public async Task<IActionResult> MyCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var courses = await _context.Courses
                .Where(c => c.Professor.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
            return View(courses);
        }

        public async Task<IActionResult> MyEnrollments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var courses = await _context.Courses
                .Where(c => c.Professor.UserId == userId)
                .Include(c => c.Enrollments)
                .AsNoTracking()
                .ToListAsync();
            return View(courses);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
