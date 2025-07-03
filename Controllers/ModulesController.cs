using EducationalPlatformApi.Core.DTOs;
using EducationalPlatformApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPlatformApi.Controllers;

[ApiController]
[Route("api/modules")]
public class ModulesController : ControllerBase
{
    private readonly CourseService _courseService;
    public ModulesController(CourseService courseService) => _courseService = courseService;

    [HttpPost("{moduleId}/lessons")]
    public async Task<IActionResult> AddLesson(Guid moduleId, CreateLessonDto dto)
    {
        try
        {
            await _courseService.AddLessonToModuleAsync(moduleId, dto);
            return Ok();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message });}
    }
}