using AutoWrapper.Wrappers;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using University.Core.Exceptions;

namespace UniversitySystem_3LayerArch.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if(context.Exception is NotFoundException)
            {
                context.Result = Response(context.Exception.Message, "item not found", StatusCodes.Status404NotFound);
                return;
            }
            if (context.Exception is BussinessException businessException)
            {
                if(businessException.Errors.Any())
                {
                    context.Result = Response(businessException.Errors, "one or more business errors occured", StatusCodes.Status400BadRequest);
                }
                else
                {
                    context.Result = Response(businessException.Message, "one or more business errors occured", StatusCodes.Status400BadRequest);
                }
            }
            if(context.Exception is ArgumentNullException)
            {
                context.Result = Response(context.Exception.Message, "Missing data", StatusCodes.Status400BadRequest);
            }
            if(context.Exception is UnauthorizedAccessException)
            {
                context.Result = Response(context.Exception.Message, "Unauthorized access", StatusCodes.Status401Unauthorized);
            }
            
            context.Result = Response(context.Exception.Message, "unrecognized error", StatusCodes.Status500InternalServerError, context.Exception.StackTrace);
            
        }


        public ObjectResult Response(string message, string title, int status, string?stackTrace=null)
        {
            var result = new ApiResponse
            {
                StatusCode = status,
                Message = title,
                ResponseException = title,
                IsError = true,
                Version = "1.0",
                Result = stackTrace
            };
            return new ObjectResult(result){
                StatusCode = status
            };
        }

        public ObjectResult Response(Dictionary<string, List<string>> errors, string title, int status)
        {
            var result = new ApiResponse
            {
                StatusCode = status,
                Message = title,
                ResponseException = title,
                IsError = true,
                Version = "1.0",
                Result = status
            };
            return new ObjectResult(result)
            {
                StatusCode = status
            };
        }
    }
}
