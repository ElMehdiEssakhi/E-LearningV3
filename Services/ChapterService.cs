using E_LearningV3.Models;
using Microsoft.EntityFrameworkCore;

namespace E_LearningV3.Services
{
    public class ChapterService
    {
        private readonly AppDbContext _context;

        public ChapterService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Chapter>> GetChaptersByCourseId(int courseId)
        {
            return await _context.Chapters
                .Where(ch => ch.CourseId == courseId)
                .OrderBy(ch => ch.Title)
                .ToListAsync();
        }
    }
}
