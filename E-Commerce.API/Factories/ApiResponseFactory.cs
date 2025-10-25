using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace E_Commerce.API.Factories
{
    public class ApiResponseFactory
    {
        public static IActionResult CustomValidationErrorResponse(ActionContext context)
        {
            //context => errors , key[Fields]
            //context.ModelState <string, ModelStateEntry>
            //string => name of the field
            //ModelStateEntry => Errors => Error Messages


            //IEnumerable<ValidationError>
            var errors = context.ModelState.Where(error => error.Value?.Errors.Any() == true).Select(error => new ValidationError
            {
                Field = error.Key,
                Errors = error.Value?.Errors.Select(error => error.ErrorMessage) ?? new List<string>()
            });
            var response = new ValidationErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ErrorMessage = "One Or More Validation Error Happend",
                Errors = errors
            };

            return new BadRequestObjectResult(response);
        }
    }
}
