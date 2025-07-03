using AutoMapper;
using EducationalPlatformApi.Core.DTOs;
using EducationalPlatformApi.Core.Entities;
using EducationalPlatformApi.Infrastructure.Repositories;

namespace EducationalPlatformApi.Services;

public class CourseService
{
    private readonly CourseRepository _courseRepository;
    private readonly InstructorRepository _instructorRepository;
    private readonly ModuleRepository _moduleRepository;
    private readonly LessonRepository _lessonRepository;
    private readonly IMapper _mapper;

    public CourseService(
        CourseRepository courseRepository,
        InstructorRepository instructorRepository,
        ModuleRepository moduleRepository,
        LessonRepository lessonRepository,
        IMapper mapper)
    {
        _courseRepository = courseRepository;
        _instructorRepository = instructorRepository;
        _moduleRepository = moduleRepository;
        _lessonRepository = lessonRepository;
        _mapper = mapper;
    }

    // --- MÉTODO CORREGIDO ---
    public async Task AddLessonToModuleAsync(Guid courseId, Guid moduleId, CreateLessonDto dto)
    {
        // 1. Validar que el curso exista y no esté publicado.
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) throw new KeyNotFoundException("Course not found.");
        if (course.IsPublished) throw new InvalidOperationException("Cannot add lessons to a published course.");

        // 2. Validar que el módulo exista.
        var module = await _moduleRepository.GetByIdAsync(moduleId);
        if (module == null) throw new KeyNotFoundException("Module not found.");
        
        // 3. Crear la nueva lección de forma explícita.
        var lesson = new Lesson(dto.Title)
        {
            ModuleId = moduleId
        };

        // 4. Llamar al repositorio de lecciones para guardarla directamente.
        await _lessonRepository.AddAsync(lesson);
    }
    
    // (El resto de los métodos del servicio permanecen igual)
    public async Task<Module> AddModuleToCourseAsync(Guid courseId, CreateModuleDto dto)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) throw new KeyNotFoundException("Course not found.");
        if (course.IsPublished) throw new InvalidOperationException("Cannot add modules to a published course.");
        var module = new Module(dto.Title) { CourseId = courseId };
        await _moduleRepository.AddAsync(module);
        return module;
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

    public async Task PublishCourseAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) throw new KeyNotFoundException("Course not found.");
        course.Publish();
        await _courseRepository.UpdateAsync(course);
    }
    
    public async Task RemoveInstructorFromCourseAsync(Guid courseId, Guid instructorId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) throw new KeyNotFoundException("Course not found.");
        var instructor = await _instructorRepository.GetByIdAsync(instructorId);
        if (instructor == null) throw new KeyNotFoundException("Instructor not found.");
        course.RemoveInstructor(instructor);
        await _courseRepository.UpdateAsync(course);
    }
}