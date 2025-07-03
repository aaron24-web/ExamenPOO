using EducationalPlatformApi.Infrastructure.Data;
using EducationalPlatformApi.Infrastructure.Repositories;
using EducationalPlatformApi.Mappings;
using EducationalPlatformApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Añadir servicios al contenedor.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("EducationalPlatformDb"));

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Repositorios
builder.Services.AddScoped<InstructorRepository>();
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<ModuleRepository>();
builder.Services.AddScoped<LessonRepository>();

// Servicios
builder.Services.AddScoped<InstructorService>();
builder.Services.AddScoped<CourseService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Este necesita Swashbuckle.AspNetCore

var app = builder.Build();

// 2. Configurar el pipeline de peticiones.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Este necesita Swashbuckle.AspNetCore
    app.UseSwaggerUI(); // Este necesita Swashbuckle.AspNetCore
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();