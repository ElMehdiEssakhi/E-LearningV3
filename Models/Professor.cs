using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_LearningV3.Models
{
    public class Professor
    {
        // Primary Key (Surrogate Key)
        [Key]
        public int ProfessorId { get; set; }

        // Foreign Key to AspNetUsers.Id (Required, one-to-one relationship)
        [Required]
        public string UserId { get; set; }

        // Navigation Property back to the Identity User
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        // Navigation Property for Courses taught by this Professor
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
