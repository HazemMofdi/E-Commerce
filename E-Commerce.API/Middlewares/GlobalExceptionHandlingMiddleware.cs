using Domain.Entities;
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
            context.Response.ContentType = "application/json";

            var response = new ErrorDetails
            {
                ErrorMessage = ex.Message
            };
            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                ValidationException validationException => HandleValidationException(validationException, response),
                (_) => StatusCodes.Status500InternalServerError
            };
            response.StatusCode = context.Response.StatusCode;

            // NOOOOOOOOTE THAT WE DID AN OVERRIDE TO THE ToString() WE USED THE JsonSerializer.Serialize(this) //
            await context.Response.WriteAsync(response.ToString());

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

        private int HandleValidationException(ValidationException validationException, ErrorDetails response)
        {
            response.Errors = validationException.Errors;
            return StatusCodes.Status400BadRequest;
        }
    }
}
