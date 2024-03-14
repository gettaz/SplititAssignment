using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SplititAssignment.Exceptions;
using SplititAssignment.Models;


public class CustomResponseFilterAttribute : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.TryGetValue("rankStart", out var rankStartValue) && rankStartValue is int rankStart)
        {
            if (rankStart < 0)
            {
                context.Result = new BadRequestObjectResult("rank Start must be 0 or greater.");
                return;
            }
        }

        if (context.ActionArguments.TryGetValue("rankEnd", out var rankEndValue) && rankEndValue is int rankEnd)
        {
            if (rankEnd < 0)
            {
                context.Result = new BadRequestObjectResult("rank End must be 0 or greater.");
                return;
            }
        }

        if (rankStartValue is int start && rankEndValue is int end)
        {
            if (end < start)
            {
                context.Result = new BadRequestObjectResult("rank End must be greater or equal to rank Start.");
                return; 
            }
        }

        if (context.ActionArguments.TryGetValue("skip", out var skipValue) && skipValue is int skip)
        {
            if (skip < 0)
            {
                context.Result = new BadRequestObjectResult("skip must be 0 or greater.");
                return;
            }
        }

        if (context.ActionArguments.TryGetValue("take", out var takeValue) && takeValue is int take)
        {
            if (take < 0)
            {
                context.Result = new BadRequestObjectResult("take must be 0 or greater.");
                return;
            }
        }

        foreach (var parameter in context.ActionArguments)
        {
            if (parameter.Value is string && string.IsNullOrWhiteSpace((string)parameter.Value))
            {
                context.Result = new BadRequestObjectResult($"{parameter.Key} must not be empty.");
                return;
            }
        }
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
