using EducationalPlatformApi.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using EducationalPlatformApi.Infrastructure.Repositories;
using EducationalPlatformApi.Mappings;
using EducationalPlatformApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuración de CORS ---
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins, policy =>
    {
        // En producción, es mejor especificar los dominios permitidos.
        // Para desarrollo, puedes usar WithOrigins("http://localhost:3000") o similar.
        policy.WithOrigins("187.155.101.200") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --- 2. Inyección de Dependencias (Tus servicios y repositorios) ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("EducationalPlatformDb"));

// Registra AutoMapper buscando perfiles en el ensamblado actual
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Registra tus repositorios y servicios para que puedan ser inyectados
builder.Services.AddScoped<InstructorRepository>();
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<ModuleRepository>();
builder.Services.AddScoped<LessonRepository>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<InstructorService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// --- 3. Configuración de Autenticación JWT ---
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

// --- 4. Configuración de Swagger para usar JWT ---
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Educational Platform API", Version = "v1" });
    
    // Define el esquema de seguridad "Bearer" para la autorización
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, introduce 'Bearer' seguido de un espacio y el token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Hace que todos los endpoints requieran el token en la UI de Swagger
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

// --- 5. Configuración del Pipeline de Peticiones HTTP ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aplica la política de CORS que definiste
app.UseCors(myAllowSpecificOrigins);

// El orden es crucial: primero se autentica y luego se autoriza.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();