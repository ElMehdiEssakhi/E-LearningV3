using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_LearningV3.Models
{
    public class ExamFinal
    {
        [Key]
        public int ExamFinalId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = null!;

        [Column(TypeName = "nvarchar(max)")]
        public string QuestionsJson { get; set; } = null!;

        [Column(TypeName = "nvarchar(max)")]
        public string AnswersJson { get; set; } = null!;

        // Foreign Key to Course
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; } = null!;
    }
}
