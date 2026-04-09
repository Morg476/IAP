using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using starter_code;
using starter_code.Data;
using starter_code.Models;
using starter_code.Middleware;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Add controllers and ignore JSON reference loops
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Set up the SQLite database
builder.Services.AddDbContext<EventAppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// Set up JWT authentication
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
        };
    });

// Enable role-based authorization
builder.Services.AddAuthorization();

// Allow requests from the frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();

// Set up Swagger and JWT support
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Event Booking API",
        Version = "v1",
        Description = "API for managing events, bookings, and comments",
        Contact = new OpenApiContact
        {
            Name = "Morgan Osborn",
            Email = "N1310588@my.ntu.ac.uk"
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token only"
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
            new string[] {}
        }
    });
});

var app = builder.Build();

// Create the database and seed some starter data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EventAppDbContext>();
    db.Database.EnsureCreated();

    // Seed starter events
    if (!db.Events.Any())
    {
        db.Events.AddRange(
            new starter_code.Models.Event
            {
                Title = "Music Festival",
                Category = "Music",
                Location = "Central Park"
            },
            new starter_code.Models.Event
            {
                Title = "Tech Expo",
                Category = "Technology",
                Location = "City Hall"
            }
        );

        db.SaveChanges();
    }

    // Seed an admin account for testing and management
    if (!db.Users.Any(u => u.Email == "admin@site.com"))
    {
        db.Users.Add(new User
        {
            Name = "Admin",
            Email = "admin@site.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            Role = "Admin"
        });

        db.SaveChanges();
    }
}

// Enable Swagger
app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Booking API v1");
    options.RoutePrefix = "swagger";
});

// Start the MK5202 initialiser
Initialiser.Start();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

// Redirect the root URL to the homepage
app.UseRedirectRoot();

app.MapRazorPages();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

// Run the API checker and return the results
app.MapGet("/api/checker/run", async (IConfiguration config) =>
{
    var baseUrl = config["ApiChecker:BaseUrl"] ?? "http://localhost:5201";
    var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "api_results.txt");

    await ApiChecker.ApiChecker.RunTests(baseUrl, outputPath);

    var results = await File.ReadAllTextAsync(outputPath);
    return Results.Text(results, "text/plain");
});

app.Run();