using Swashbuckle.AspNetCore.SwaggerUI;

namespace TN.Web.Configs
{
    public static class ApplicationConfiguration
    {
        public static void Inject(WebApplication app)
        {

            app.Use((context, next) =>
            {
                context.Response.Headers.Remove("X-Powered-By");
                context.Response.Headers.Remove("Server");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                //context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                //context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                //context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                //context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
                return next();
            });

            #region RedirectSwagger
            //Redirect request from the root Url to swagger UI
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/swagger/index.html");
                    return;
                }

                await next();

            });
            #endregion

            // Enable Swagger
            if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = "swagger"; // URL path to access Swagger (swagger/index.html)
                });
            }



          



          

        }
    }
}
