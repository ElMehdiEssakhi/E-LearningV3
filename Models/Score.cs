using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_LearningV3.Models
{
    public class Score
    {
        [Key]
        public int ScoreId { get; set; }

        public DateTime PassedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int ScoreValue { get; set; } // Renamed from 'score' to 'ScoreValue' for clarity

        // Foreign Key to Quiz
        public int QuizId { get; set; }
        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; } = null!;

        // Foreign Key to Student
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; } = null!;
    }
}
