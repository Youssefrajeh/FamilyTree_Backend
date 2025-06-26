using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FamilyTreeAPI.Data;
using FamilyTreeAPI.Models;
using FamilyTreeAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Entity Framework - try PostgreSQL first, fallback to In-Memory for testing
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var useInMemory = builder.Configuration.GetValue<bool>("UseInMemoryDatabase", false);

if (useInMemory || string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("🧪 Using In-Memory Database for testing");
    builder.Services.AddDbContext<FamilyTreeContext>(options =>
        options.UseInMemoryDatabase("FamilyTreeTest"));
}
else
{
    try
    {
        Console.WriteLine("🗄️ Attempting to connect to PostgreSQL...");
        builder.Services.AddDbContext<FamilyTreeContext>(options =>
            options.UseNpgsql(connectionString));
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ PostgreSQL connection failed: {ex.Message}");
        Console.WriteLine("🧪 Falling back to In-Memory Database");
        builder.Services.AddDbContext<FamilyTreeContext>(options =>
            options.UseInMemoryDatabase("FamilyTreeTest"));
    }
}

// Add Identity services
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<FamilyTreeContext>()
.AddDefaultTokenProviders();

// Add JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "test-super-secret-jwt-key-for-local-development-only-32-chars";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "FamilyTreeAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "FamilyTreeApp";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero
    };
});

// Add application services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFamilyTreeService, FamilyTreeService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins(
            "http://localhost:9000",
            "http://localhost:9001",
            "http://localhost:8080",
            "http://127.0.0.1:9000",
            "http://127.0.0.1:9001"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Family Tree API",
        Version = "v1",
        Description = "API for managing family tree data and relationships"
    });

    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Family Tree API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

// Enable CORS
app.UseCors("AllowLocalhost");

// Security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Create database and seed data for testing
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FamilyTreeContext>();

    try
    {
        if (context.Database.IsInMemory())
        {
            Console.WriteLine("🧪 Ensuring In-Memory database is created...");
            await context.Database.EnsureCreatedAsync();
        }
        else
        {
            Console.WriteLine("🗄️ Applying database migrations...");
            await context.Database.MigrateAsync();
        }

        Console.WriteLine("✅ Database setup completed successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database setup error: {ex.Message}");
    }
}

// Add a simple health check endpoint
app.MapGet("/health", () => new
{
    status = "healthy",
    timestamp = DateTime.UtcNow,
    database = app.Services.GetRequiredService<FamilyTreeContext>().Database.IsInMemory() ? "in-memory" : "postgresql"
});

Console.WriteLine("🚀 Family Tree API is starting...");
Console.WriteLine($"🌐 Environment: {app.Environment.EnvironmentName}");
Console.WriteLine($"🔑 JWT Issuer: {jwtIssuer}");

app.Run();
