using EllipticCurve;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Infrastructure.CustomMiddleware.CustomException;

namespace TN.Shared.Infrastructure.CustomMiddleware.GlobalErrorHandling
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionMiddleware> logger)
        {
            _next = requestDelegate;
            _logger = logger;
            
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }catch(UnauthorizedExceptions ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.Unauthorized);
               }
            catch (DirectoryNotFoundException ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.NotFound);
            }
            catch (NotFoundExceptions ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.InternalServerError);
            }
        }


        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex, HttpStatusCode statusCode)
        {
            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = "application/json";

            _logger.LogError(ex, "An exception occurred: {Exception}", ex);
            await httpContext.Response.WriteAsync(new ErrorDetails
            (
                 (int)statusCode,
                ex.Message
                ).ToString());
           
        }
    }
    
}
