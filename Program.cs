using EducationalPlatformApi.Infrastructure.Data;
using EducationalPlatformApi.Infrastructure.Repositories;
using EducationalPlatformApi.Mappings;
using EducationalPlatformApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Servicios ---
// Añade aquí todas las configuraciones de servicios.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_myAllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:7233")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("EducationalPlatformDb"));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<InstructorRepository>();
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<ModuleRepository>();
builder.Services.AddScoped<LessonRepository>();

builder.Services.AddScoped<InstructorService>();
builder.Services.AddScoped<CourseService>();

builder.Services.AddControllers();

// Configuración de Autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Introduce 'Bearer' [espacio] y tu token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            System.Array.Empty<string>()
        }
    });
});


// --- Construir la aplicación ---
var app = builder.Build();


// --- 2. Pipeline de Peticiones ---
// El orden aquí es CRÍTICO para que la seguridad funcione.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Primero se aplica la política de CORS.
app.UseCors("_myAllowSpecificOrigins");

// ¡ORDEN CRÍTICO!
// 1. PRIMERO, la aplicación debe saber qué es "autenticar".
app.UseAuthentication();

// 2. LUEGO, debe verificar si el usuario autenticado tiene permisos ("autorización").
app.UseAuthorization();

// 3. FINALMENTE, una vez verificada la seguridad, se dirige la petición al controlador correspondiente.
app.MapControllers();


// --- Ejecutar la aplicación ---
app.Run();