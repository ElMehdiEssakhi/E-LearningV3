using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_LearningV3.Models
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = null!;

        // JSON will be stored as a string in the database
        [Column(TypeName = "nvarchar(max)")]
        public string QuestionsJson { get; set; } = null!;

        [Column(TypeName = "nvarchar(max)")]
        public string AnswersJson { get; set; } = null!;

        // Foreign Key to Chapter
        public int ChapId { get; set; }
        [ForeignKey("ChapId")]
        public Chapter Chapter { get; set; } = null!;

        // Navigation Property
        public ICollection<Score> Scores { get; set; } = new List<Score>();
    }
}
