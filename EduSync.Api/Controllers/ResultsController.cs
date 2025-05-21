using EduSync.Infrastructure.Data;
using EduSync.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResultsController : ControllerBase
{
    private readonly DataContext _context;
    public ResultsController(DataContext context) => _context = context;

    [HttpGet]
    public IActionResult GetMyResults()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var results = _context.Results
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.AttemptDate)
            .Select(r => new {
                r.CourseId,
                r.AssessmentId,
                r.Score,
                r.MaxScore,
                r.AttemptDate
            })
            .ToList();
        return Ok(results);
    }
}