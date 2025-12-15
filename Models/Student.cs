using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_LearningV3.Models
{
    public class Student
    {
        // Primary Key (Surrogate Key)
        [Key]
        public int StudentId { get; set; }

        // Foreign Key to AspNetUsers.Id (Required, one-to-one relationship)
        [Required]
        public string UserId { get; set; }

        // Navigation Property back to the Identity User
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        // Navigation Property for Enrollments
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        // Navigation Property for Scores
        public ICollection<Score> Scores { get; set; } = new List<Score>();

        // Navigation Property for Certificates
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

        // Navigation Property for Chapter Progress
        //public ICollection<StudentChapterProgress> ChapterProgress { get; set; } = new List<StudentChapterProgress>();
    }
}
