using EducationalPlatformApi.Core.DTOs;
using EducationalPlatformApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EducationalPlatformApi.Controllers;

[ApiController]
[Route("api/courses")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly CourseService _courseService;
    public CoursesController(CourseService courseService) => _courseService = courseService;

    [HttpPost("{courseId}/modules")]
    public async Task<IActionResult> AddModule(Guid courseId, CreateModuleDto dto)
    {
        try
        {
            await _courseService.AddModuleToCourseAsync(courseId, dto);
            return Ok();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message });}
    }

    [HttpPost("{courseId}/publish")]
    public async Task<IActionResult> Publish(Guid courseId)
    {
        try
        {
            await _courseService.PublishCourseAsync(courseId);
            return NoContent();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message });}
    }
}