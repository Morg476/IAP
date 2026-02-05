using api;
using starter_code;
using starter_code.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers(); // ✅ already needed

// Register custom API services
builder.RegisterApi(
    Initialiser.GetDir(builder.Configuration.GetValue<string>("DbFile") ?? "")
);

var app = builder.Build();

// ✅ Minimal API endpoint
app.MapGet("/", () => "Hello World!");

// Map controllers
app.MapControllers();

// Start Mk5202 Initialiser
Initialiser.Start();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseApi();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseRedirectRoot();

app.MapRazorPages();

app.Run();
