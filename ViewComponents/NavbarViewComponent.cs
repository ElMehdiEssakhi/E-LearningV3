using Microsoft.AspNetCore.Mvc;

namespace E_LearningV3.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Check if the user is authenticated and in the "Student" role
            if (User.Identity.IsAuthenticated && User.IsInRole("Student"))
            {
                return View();
            }

            // Return an empty content if they aren't a student
            return Content(string.Empty);
        }
    }
    
}
