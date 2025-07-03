using EducationalPlatformApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EducationalPlatformApi.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
}