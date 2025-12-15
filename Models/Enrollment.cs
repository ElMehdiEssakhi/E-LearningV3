using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_LearningV3.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentID { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string CompletionStatus { get; set; } = "In Progress"; // e.g., "In Progress", "Completed"

        [Range(0, 100)]
        public int ProgressPercentage { get; set; } // Course overall progress

        // Foreign Key to Student
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; } = null!;

        // Foreign Key to Course
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; } = null!;
    }
}
