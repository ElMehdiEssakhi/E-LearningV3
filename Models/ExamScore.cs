using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_LearningV3.Models
{
    public class ExamScore
    {
        [Key]
        public int ExamScoreId { get; set; }

        public int ExamFinalId { get; set; }
        [ForeignKey("ExamFinalId")]
        public ExamFinal ExamFinal { get; set; } = null!;

        public int StudentId { get; set; }

        public int ScoreValue { get; set; } // e.g., 0-100

        public DateTime TakenAt { get; set; } = DateTime.Now;
    }
}
