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
using System.Text.Json.Serialization; // <-- 1. AÑADE ESTE USING

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuración de CORS ---
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("187.155.101.200") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --- 2. Inyección de Dependencias ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("EducationalPlatformDb"));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<InstructorRepository>();
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<ModuleRepository>();
builder.Services.AddScoped<LessonRepository>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<InstructorService>();

// --- 2. MODIFICA ESTA LÍNEA ---
// Se añade la configuración para manejar ciclos en las referencias de objetos
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

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
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, introduce 'Bearer' seguido de un espacio y el token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
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
app.UseCors(myAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();