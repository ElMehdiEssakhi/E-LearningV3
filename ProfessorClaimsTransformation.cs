using System.Security.Claims;
using E_LearningV3;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

public class PrStClaimsTransformation : IClaimsTransformation
{
    private readonly AppDbContext _context;

    public PrStClaimsTransformation(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (!principal.Identity!.IsAuthenticated) return principal;

        var clone = principal.Clone();
        var newIdentity = (ClaimsIdentity)clone.Identity!;
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null) return principal;

        // 1. Handle Professor Claim
        if (!principal.HasClaim(c => c.Type == "ProfessorId"))
        {
            var profId = await _context.Professors
                .Where(p => p.UserId == userId)
                .Select(p => p.ProfessorId)
                .FirstOrDefaultAsync();

            if (profId != 0)
                newIdentity.AddClaim(new Claim("ProfessorId", profId.ToString()));
        }

        // 2. Handle Student Claim
        if (!principal.HasClaim(c => c.Type == "StudentId"))
        {
            var studentId = await _context.Students
                .Where(s => s.UserId == userId)
                .Select(s => s.StudentId)
                .FirstOrDefaultAsync();

            if (studentId != 0)
                newIdentity.AddClaim(new Claim("StudentId", studentId.ToString()));
        }

        return clone;
    }
}
