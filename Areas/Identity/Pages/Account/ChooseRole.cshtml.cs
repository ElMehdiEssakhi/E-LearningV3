using Microsoft.AspNetCore.Mvc.RazorPages;

namespace E_LearningV3.Areas.Identity.Pages.Account
{
    public class ChooseRoleModel : PageModel
    {
        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            // Capture the returnUrl, which can be passed to the Register page links
            ReturnUrl = returnUrl;
        }
    }
}
