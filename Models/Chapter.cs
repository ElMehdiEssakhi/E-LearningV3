using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_LearningV3.Models
{
    public class Chapter
    {
        [Key]
        public int ChapId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = null!;

        // Foreign Key to Course
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        [ValidateNever] // prevents validation on navigation property
        public Course Course { get; set; } = null!;

        // Navigation Properties
        [ValidateNever]
        public ICollection<Content> Contents { get; set; } = new List<Content>();

        [ValidateNever]
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}
