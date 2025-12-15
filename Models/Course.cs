using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_LearningV3.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [StringLength(200)]
        public string CourseName { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        // Foreign Key to Professor
        public int ProfessorId { get; set; }
        [ForeignKey("ProfessorId")]
        public Professor Professor { get; set; } = null!;

        // Navigation Properties
        public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<ExamFinale> ExamFinales { get; set; } = new List<ExamFinale>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
    }
}
