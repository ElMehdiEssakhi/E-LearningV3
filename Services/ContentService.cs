using E_LearningV3.Models;
using Microsoft.EntityFrameworkCore;

namespace E_LearningV3.Services
{
    public class ContentService
    {
        private readonly AppDbContext _context;

        public ContentService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Content>> GetContentsByChapterId(int chapId)
        {
            return await _context.Contents
                .Where(c => c.ChapId == chapId)
                .OrderBy(c => c.Type)
                .ToListAsync();
        }
    }
}
