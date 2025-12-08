using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_LearningV3.Models
{
    public class User : IdentityUser
    {
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
