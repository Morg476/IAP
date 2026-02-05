using api;
using starter_code;
using starter_code.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllers();

// ✅ ADD THIS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Register custom API services
builder.RegisterApi(
    Initialiser.GetDir(builder.Configuration.GetValue<string>("DbFile") ?? "")
);

var app = builder.Build();

// Start Mk5202 Initialiser
Initialiser.Start();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ MOVE HERE (after routing)
app.UseCors();

app.UseAuthorization();

app.UseApi();

app.UseRedirectRoot();

app.MapRazorPages();

// Map controllers
app.MapControllers();

// Optional
app.MapGet("/", () => "Hello World!");

app.Run();
