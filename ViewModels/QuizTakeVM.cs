namespace E_LearningV3.ViewModels
{
    public class QuizTakeVM
    {
        public int QuizId { get; set; }
        public int ChapterId { get; set; }
        public int CourseId { get; set; }
        public List<QuizQuestionVM> Questions { get; set; } = new();
        public Dictionary<int, string> UserAnswers { get; set; } = new();
    }
}
