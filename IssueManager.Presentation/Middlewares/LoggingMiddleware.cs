namespace IssueManager.Presentation.Middlewares
{
    public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            logger.LogInformation("Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);

            try
            {
                await next(context);
                logger.LogInformation("Response status: {StatusCode}", context.Response.StatusCode);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the request");
                throw;
            }
        }
    }
}
