using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Infrastructure.CustomMiddleware.GlobalErrorHandling
{
    public static class ErrorHandlingMiddlewareExtension
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
