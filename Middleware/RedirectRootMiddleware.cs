namespace starter_code.Middleware;

// Middleware to redirect the root URL to the main homepage
public class RedirectRootMiddleware
{
    private readonly RequestDelegate _next;

    // Constructor receives the next middleware in the pipeline
    public RedirectRootMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // Intercepts incoming requests
    public async Task InvokeAsync(HttpContext context)
    {
        // Redirect requests from "/" to "index.html"
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/index.html");
            return;
        }

        // Pass control to the next middleware
        await _next(context);
    }
}

// Extension method for cleaner middleware registration
public static class RedirectRootMiddlewareExtensions
{
    // Enables the middleware via app.UseRedirectRoot()
    public static IApplicationBuilder UseRedirectRoot(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RedirectRootMiddleware>();
    }
}