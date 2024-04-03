﻿using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Api.Middlewares;

internal sealed class ApiExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionHandlingMiddleware> _logger;

    public ApiExceptionHandlingMiddleware(RequestDelegate next, ILogger<ApiExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        string result=string.Empty;

        if (ex is DomainException e)
        {
            //var problemDetails = new CustomValidationProblemDetails(new List<ValidationError> { new() { Code = e.Code, Message = e.Message } })
            //{
            //    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            //    Title = "One or more validation errors occurred.",
            //    Status = (int)HttpStatusCode.BadRequest,
            //    Instance = context.Request.Path,
            //};
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
          //  result = JsonSerializer.Serialize(problemDetails);
        }
        else
        {
            _logger.LogError(ex, $"An unhandled exception has occurred, {ex.Message}");
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "Internal Server Error.",
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = context.Request.Path,
                Detail = "Internal server error occurred!"
            };
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            result = JsonSerializer.Serialize(value:problemDetails);
        }

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
}
