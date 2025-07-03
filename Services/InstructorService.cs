using AutoMapper;
using EducationalPlatformApi.Core.DTOs;
using EducationalPlatformApi.Core.Entities;
using EducationalPlatformApi.Infrastructure.Repositories;

namespace EducationalPlatformApi.Services;

public class InstructorService
{
    private readonly InstructorRepository _instructorRepository;
    private readonly IMapper _mapper;

    public InstructorService(InstructorRepository instructorRepository, IMapper mapper)
    {
        _instructorRepository = instructorRepository;
        _mapper = mapper;
    }

    public async Task<Instructor> CreateInstructorAsync(CreateInstructorDto dto)
    {
        if (await _instructorRepository.GetByNameAsync(dto.Name) != null)
            throw new InvalidOperationException("An instructor with this name already exists.");
        
        var instructor = _mapper.Map<Instructor>(dto);
        await _instructorRepository.AddAsync(instructor);
        return instructor;
    }

    public async Task DeleteInstructorAsync(Guid id)
    {
        var instructor = await _instructorRepository.GetByIdWithCoursesAsync(id);
        if (instructor == null) throw new KeyNotFoundException("Instructor not found.");

        if (instructor.Courses.Any(c => c.IsPublished))
            throw new InvalidOperationException("Cannot delete an instructor assigned to a published course.");
        
        await _instructorRepository.DeleteAsync(instructor);
    }

    public async Task<Instructor?> GetInstructorByIdAsync(Guid id) => 
        await _instructorRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Instructor>> GetAllInstructorsAsync() => 
        await _instructorRepository.GetAllAsync();

    public async Task UpdateInstructorAsync(Guid id, UpdateInstructorDto dto)
    {
        var instructor = await _instructorRepository.GetByIdAsync(id);
        if (instructor == null) throw new KeyNotFoundException("Instructor not found.");
        
        _mapper.Map(dto, instructor);
        await _instructorRepository.UpdateAsync(instructor);
    }
}