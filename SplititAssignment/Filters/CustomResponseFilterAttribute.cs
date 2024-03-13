using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SplititAssignment.Exceptions;

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
                DuplicateRankException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            var errorResponse = new
            {
                Errors = new List<object>
                {
                    new
                    {
                        (context.Exception as CustomException)?.Code,
                        context.Exception.Message,
                        (context.Exception as CustomException)?.AdditionalInfo
                    }
                },
                StatusCode = status
            ,
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
