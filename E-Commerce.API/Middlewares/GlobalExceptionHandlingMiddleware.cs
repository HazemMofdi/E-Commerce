using Domain.Exceptions;
using Shared.ErrorModels;
using System.Net;
using System.Text.Json;

namespace E_Commerce.API.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
         {
            try
            {
                // 404 product not found throw exception, 500 internal server error 
                await next(context); 
                // --------- // 404 not found for the path --------- //
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await HandleNotFoundApiAsync(context);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Something Went Wrong ==> {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleNotFoundApiAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"The Endpoint With Url {context.Request.Path} Is Not Found",
                
            }.ToString();
            await context.Response.WriteAsync(response);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //1 Change StatusCode
            //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                (_) => StatusCodes.Status500InternalServerError
            };

            //2 Change ContentType
            context.Response.ContentType = "application/json";

            //3 Write Response In The Body
            var response = new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = ex.Message
            }.ToString();
            await context.Response.WriteAsync(response);

            #region THREE WAYS TO SERIALIZE THE INSTANCE
            #region 1
            //await context.Response.WriteAsync(response);
            #endregion
            #region 2
            //var result = JsonSerializer.Serialize(response);
            //await context.Response.WriteAsync(result); 
            #endregion
            #region 3
            //await context.Response.WriteAsJsonAsync(response); 
            #endregion 
            #endregion
        }
    }
}
