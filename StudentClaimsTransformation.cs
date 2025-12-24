using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using E_LearningV3;

public class StudentClaimsTransformation : IClaimsTransformation
{
    private readonly AppDbContext _context;

    public StudentClaimsTransformation(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (!principal.Identity!.IsAuthenticated)
            return principal;

        // Avoid duplicate claim
        if (principal.HasClaim(c => c.Type == "StudentId"))
            return principal;

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return principal;

        var studentId = await _context.Students
            .Where(s => s.UserId == userId)
            .Select(s => s.StudentId)
            .FirstOrDefaultAsync();

        if (studentId == 0)
            return principal;

        var identity = (ClaimsIdentity)principal.Identity;
        identity.AddClaim(new Claim("StudentId", studentId.ToString()));

        return principal;
    }
}
