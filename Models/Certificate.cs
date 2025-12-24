using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_LearningV3.Models
{
    public class Certificate
    {
        [Key]
        public int CertificateId { get; set; }

        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string FilePath { get; set; } = null!; // Link to the PDF/image certificate

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
