using AutoMapper;
using EducationalPlatformApi.Core.DTOs;
using EducationalPlatformApi.Core.Entities;
using EducationalPlatformApi.Infrastructure.Repositories;

namespace EducationalPlatformApi.Services;

public class CourseService
{
    private readonly CourseRepository _courseRepository;
    private readonly InstructorRepository _instructorRepository;
    private readonly IMapper _mapper;

    public CourseService(
        CourseRepository courseRepository,
        InstructorRepository instructorRepository,
        IMapper mapper)
    {
        _courseRepository = courseRepository;
        _instructorRepository = instructorRepository;
        _mapper = mapper;
    }

    public async Task<Course> CreateCourseAsync(CreateCourseDto dto)
    {
        var course = new Course(dto.Title, dto.Description);

        if (dto.InstructorIds.Any())
        {
            var instructors = await _instructorRepository.GetByIdsAsync(dto.InstructorIds);
            if (instructors.Count != dto.InstructorIds.Count)
                throw new KeyNotFoundException("One or more instructors were not found.");
            
            foreach (var instructor in instructors)
            {
                course.AddInstructor(instructor);
            }
        }
        await _courseRepository.AddAsync(course);
        return course;
    }
    
    public async Task AddModuleToCourseAsync(Guid courseId, CreateModuleDto dto)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) throw new KeyNotFoundException("Course not found.");
        course.AddModule(dto.Title);
        await _courseRepository.UpdateAsync(course);
    }
    
    public async Task AddLessonToModuleAsync(Guid courseId, Guid moduleId, CreateLessonDto dto)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) throw new KeyNotFoundException("Course not found.");
        course.AddLessonToModule(moduleId, dto.Title);
        await _courseRepository.UpdateAsync(course);
    }

    public async Task PublishCourseAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) throw new KeyNotFoundException("Course not found.");
        course.Publish();
        await _courseRepository.UpdateAsync(course);
    }
    
    // --- MÉTODO CORREGIDO ---
    // Se añade este método de nuevo para que el controlador funcione.
    public async Task RemoveInstructorFromCourseAsync(Guid courseId, Guid instructorId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) throw new KeyNotFoundException("Course not found.");

        var instructor = await _instructorRepository.GetByIdAsync(instructorId);
        if (instructor == null) throw new KeyNotFoundException("Instructor not found.");

        // La lógica de negocio se delega a la entidad Course
        course.RemoveInstructor(instructor);

        await _courseRepository.UpdateAsync(course);
    }
}