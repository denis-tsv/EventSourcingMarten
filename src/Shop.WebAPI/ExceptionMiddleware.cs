using System.Net;
using Shop.UseCases.Exceptions;

namespace Shop.WebAPI;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    
    public ExceptionMiddleware(RequestDelegate next) => _next = next;
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException)
        {
            context.Response.StatusCode = (int) HttpStatusCode.NotFound;
        }
        catch (OptimisticLockException)
        {
            context.Response.StatusCode = (int) HttpStatusCode.Conflict;
        }

    }
}