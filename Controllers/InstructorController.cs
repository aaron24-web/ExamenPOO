using EducationalPlatformApi.Core.DTOs;
using EducationalPlatformApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPlatformApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstructorsController : ControllerBase
{
    private readonly InstructorService _instructorService;

    public InstructorsController(InstructorService instructorService)
    {
        _instructorService = instructorService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateInstructorDto dto)
    {
        try
        {
            var instructor = await _instructorService.CreateInstructorAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = instructor.Id }, instructor);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var instructor = await _instructorService.GetInstructorByIdAsync(id);
        return instructor == null ? NotFound() : Ok(instructor);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var instructors = await _instructorService.GetAllInstructorsAsync();
        return Ok(instructors);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateInstructorDto dto)
    {
        try
        {
            await _instructorService.UpdateInstructorAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _instructorService.DeleteInstructorAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}