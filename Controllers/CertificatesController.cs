using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_LearningV3.Controllers
{
    public class CertificatesController : Controller
    {
        private readonly AppDbContext _context;

        public CertificatesController(AppDbContext context)
        {
            _context = context;
        }
        // GET: Certificates/MyCertificates
        public async Task<IActionResult> MyCertificates()
        {
            // Get logged-in student id
            var studentIdClaim = User.FindFirst("StudentId")?.Value;
            if (string.IsNullOrEmpty(studentIdClaim))
                return Forbid();

            int studentId = int.Parse(studentIdClaim);

            var certificates = await _context.Certificates
                .Include(c => c.Course)
                .Where(c => c.StudentId == studentId)
                .OrderByDescending(c => c.IssuedAt)
                .ToListAsync();

            return View(certificates);
        }
        // GET: Certificates/Download/5
        public async Task<IActionResult> Download(int id)
        {
            var studentIdClaim = User.FindFirst("StudentId")?.Value;
            if (string.IsNullOrEmpty(studentIdClaim))
                return Forbid();

            int studentId = int.Parse(studentIdClaim);

            var cert = await _context.Certificates
                .FirstOrDefaultAsync(c => c.CertificateId == id && c.StudentId == studentId);

            if (cert == null)
                return NotFound();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cert.FilePath);

            if (!System.IO.File.Exists(filePath))
                return NotFound("Certificate file not found.");

            return PhysicalFile(filePath, "application/pdf", Path.GetFileName(filePath));
        }
    }
}
