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
    public Course Course { get; set; } = null!;

    // Navigation Properties
    public ICollection<Content> Contents { get; set; } = new List<Content>();
    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    //public ICollection<StudentChapterProgress> ChapterProgress { get; set; } = new List<StudentChapterProgress>();
    }
}

