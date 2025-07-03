using EducationalPlatformApi.Core.DTOs;
using EducationalPlatformApi.Infrastructure.Repositories;
using EducationalPlatformApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPlatformApi.Controllers;

[ApiController]
[Route("api/courses")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly CourseService _courseService;
    private readonly CourseRepository _courseRepository;

    public CoursesController(CourseService courseService, CourseRepository courseRepository)
    {
        _courseService = courseService;
        _courseRepository = courseRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse(CreateCourseDto dto)
    {
        try
        {
            var course = await _courseService.CreateCourseAsync(dto);
            return CreatedAtAction(nameof(GetCourseById), new { courseId = course.Id }, course);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{courseId}")]
    public async Task<IActionResult> GetCourseById(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        return course == null ? NotFound() : Ok(course);
    }

    [HttpPost("{courseId}/modules")]
    public async Task<IActionResult> AddModule(Guid courseId, CreateModuleDto dto)
    {
        try
        {
            await _courseService.AddModuleToCourseAsync(courseId, dto);
            return Ok();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
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
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }
    
    [HttpDelete("{courseId}/instructors/{instructorId}")]
    public async Task<IActionResult> RemoveInstructorFromCourse(Guid courseId, Guid instructorId)
    {
        try
        {
            await _courseService.RemoveInstructorFromCourseAsync(courseId, instructorId);
            return NoContent();
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