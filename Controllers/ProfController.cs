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
        public IActionResult Index()
        {
            return View();
        }
    }
}
