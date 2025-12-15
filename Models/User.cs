using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_LearningV3.Models
{
    public class User : IdentityUser
    {
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Add new fields for the user's name
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        // Full name property for display
        public string FullName => $"{FirstName} {LastName}";
    }
}
