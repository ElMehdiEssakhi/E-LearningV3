using E_LearningV3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_LearningV3.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly UserManager<User> _userManager;
        public StudentController(UserManager<User> userManager)
        {
            _userManager = userManager;
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

            return View();
        }
    }
}
