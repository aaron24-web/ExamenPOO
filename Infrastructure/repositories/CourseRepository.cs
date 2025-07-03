using EducationalPlatformApi.Core.Entities;
using EducationalPlatformApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationalPlatformApi.Infrastructure.Repositories;

public class CourseRepository
{
    private readonly ApplicationDbContext _context;
    public CourseRepository(ApplicationDbContext context) => _context = context;
    
    public async Task AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
    }

    public async Task<Course?> GetByIdAsync(Guid id) =>
        await _context.Courses
            .Include(c => c.Modules)
            .ThenInclude(m => m.Lessons)
            .Include(c => c.Instructors)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task UpdateAsync(Course course)
    {
        // Al haber cargado el curso desde el contexto, EF ya lo está rastreando.
        // Solo necesitamos guardar los cambios que el rastreador detecte.
        // Esto insertará, actualizará o eliminará entidades hijas según sea necesario.
        await _context.SaveChangesAsync();
    }
}