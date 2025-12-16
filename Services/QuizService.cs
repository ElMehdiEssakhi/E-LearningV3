using E_LearningV3.Models;
using Microsoft.EntityFrameworkCore;

namespace E_LearningV3.Services
{
    public class QuizService
    {
        private readonly AppDbContext _context;

        public QuizService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Quiz>> GetQuizzesByChapterId(int chapId)
        {
            return await _context.Quizzes
                .Where(q => q.ChapId == chapId)
                .OrderBy(q => q.Title)
                .ToListAsync();
        }
    }
}
