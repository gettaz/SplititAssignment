using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SplititAssignment.Exceptions;
using SplititAssignment.Models;

public class CustomResponseFilterAttribute : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {

    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception == null)
        {
            if (context.Result is ObjectResult objectResult)
            {

                var successResponse = new
                {
                    objectResult.Value,
                    Errors = (object)null,
                    StatusCode = objectResult.StatusCode ?? StatusCodes.Status200OK,
                    TraceId = context.HttpContext.TraceIdentifier,
                    IsSuccess = true
                };

                context.Result = new ObjectResult(successResponse)
                {
                    StatusCode = objectResult.StatusCode
                };
            }
        }
        else
        {
            var status = context.Exception switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                DuplicateEntityException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };
            var errors = new List<Error>
            {
                new Error
                {
                    Code = (context.Exception as CustomException)?.Code ?? "Error",
                    Message = context.Exception.Message,
                    AdditionalInfo = (context.Exception as CustomException)?.AdditionalInfo
                    }
            };

            var errorResponse = new
            {
                Errors = errors,
                StatusCode = status,
                TraceId = context.HttpContext.TraceIdentifier,
                IsSuccess = false
            };

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = status
            };

            context.ExceptionHandled = true;
        }
    }
}
