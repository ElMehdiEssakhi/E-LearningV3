using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_LearningV3.ViewModels
{
    public class FinalExamViewModel
    {
        public int ExamFinalId { get; set; }
        public string Title { get; set; } = "";

        public bool ExamPassed { get; set; }

        public List<QuestionModel> Questions { get; set; } = new List<QuestionModel>();

        [Required]
        public Dictionary<int, string> Answers { get; set; } = new Dictionary<int, string>();
    }
}