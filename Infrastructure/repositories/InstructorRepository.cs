using EducationalPlatformApi.Core.Entities;
using EducationalPlatformApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationalPlatformApi.Infrastructure.Repositories;

public class InstructorRepository
{
    private readonly ApplicationDbContext _context;
    public InstructorRepository(ApplicationDbContext context) => _context = context;

    public async Task AddAsync(Instructor instructor)
    {
        await _context.Instructors.AddAsync(instructor);
        await _context.SaveChangesAsync();
    }

    public async Task<Instructor?> GetByNameAsync(string name) =>
        await _context.Instructors.FirstOrDefaultAsync(i => i.Name.ToLower() == name.ToLower());

    public async Task<Instructor?> GetByIdAsync(Guid id) =>
        await _context.Instructors.FindAsync(id);

    public async Task<Instructor?> GetByIdWithCoursesAsync(Guid id) =>
        await _context.Instructors.Include(i => i.Courses).FirstOrDefaultAsync(i => i.Id == id);

    public async Task<List<Instructor>> GetByIdsAsync(List<Guid> ids) =>
        await _context.Instructors.Where(i => ids.Contains(i.Id)).ToListAsync();

    public async Task<IEnumerable<Instructor>> GetAllAsync() =>
        await _context.Instructors.ToListAsync();

    public async Task UpdateAsync(Instructor instructor)
    {
        _context.Instructors.Update(instructor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Instructor instructor)
    {
        _context.Instructors.Remove(instructor);
        await _context.SaveChangesAsync();
    }
}