using EducationalPlatformApi.Core.DTOs;
using EducationalPlatformApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPlatformApi.Controllers;

[ApiController]
[Route("api/courses/{courseId}/modules/{moduleId}/lessons")] // <-- URL más descriptiva y RESTful
[Authorize]
public class ModulesController : ControllerBase
{
    private readonly CourseService _courseService;

    public ModulesController(CourseService courseService) => _courseService = courseService;

    [HttpPost]
    public async Task<IActionResult> AddLessonToModule(Guid courseId, Guid moduleId, CreateLessonDto dto)
    {
        try
        {
            await _courseService.AddLessonToModuleAsync(courseId, moduleId, dto);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}