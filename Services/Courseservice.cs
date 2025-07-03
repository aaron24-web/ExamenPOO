using AutoMapper;
using EducationalPlatformApi.Core.DTOs;
using EducationalPlatformApi.Core.Entities;
using EducationalPlatformApi.Infrastructure.Repositories;

namespace EducationalPlatformApi.Services;

public class CourseService
{
    private readonly CourseRepository _courseRepository;
    private readonly ModuleRepository _moduleRepository;
    private readonly LessonRepository _lessonRepository;
    private readonly IMapper _mapper;

    public CourseService(
        CourseRepository courseRepository,
        ModuleRepository moduleRepository,
        LessonRepository lessonRepository,
        IMapper mapper)
    {
        _courseRepository = courseRepository;
        _moduleRepository = moduleRepository;
        _lessonRepository = lessonRepository;
        _mapper = mapper;
    }

    public async Task AddModuleToCourseAsync(Guid courseId, CreateModuleDto dto)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) throw new KeyNotFoundException("Course not found.");
        
        var module = _mapper.Map<Module>(dto);
        course.AddModule(module); // La lógica de negocio está en la entidad
        
        await _courseRepository.UpdateAsync(course);
    }
    
    public async Task PublishCourseAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) throw new KeyNotFoundException("Course not found.");
        
        course.Publish();
        await _courseRepository.UpdateAsync(course);
    }

    public async Task AddLessonToModuleAsync(Guid moduleId, CreateLessonDto dto)
    {
        var module = await _moduleRepository.GetByIdAsync(moduleId);
        if (module == null) throw new KeyNotFoundException("Module not found.");

        var course = await _courseRepository.GetByIdAsync(module.CourseId);
        if (course == null || course.IsPublished)
            throw new InvalidOperationException("Cannot add lessons to a module in a published course.");

        var lesson = _mapper.Map<Lesson>(dto);
        lesson.ModuleId = moduleId;
        
        await _lessonRepository.AddAsync(lesson);
    }
}