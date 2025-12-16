using System.Security.Claims;
using E_LearningV3;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

public class ProfessorClaimsTransformation : IClaimsTransformation
{
    private readonly AppDbContext _context;

    public ProfessorClaimsTransformation(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (!principal.Identity!.IsAuthenticated)
            return principal;

        // Avoid duplicate claim
        if (principal.HasClaim(c => c.Type == "ProfessorId"))
            return principal;

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return principal;

        var professorId = await _context.Professors
            .Where(p => p.UserId == userId)
            .Select(p => p.ProfessorId)
            .FirstOrDefaultAsync();

        if (professorId == 0)
            return principal;

        var identity = (ClaimsIdentity)principal.Identity;
        identity.AddClaim(new Claim("ProfessorId", professorId.ToString()));

        return principal;
    }
}
