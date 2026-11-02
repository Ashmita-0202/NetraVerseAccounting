using NV.Payment.Application;
using NV.Payment.Infrastructure;
using OfficeOpenXml;
using Tn.Reports.Application;
using TN.Account.Application;
using TN.Account.Infrastructure;
using TN.Authentication.Application;
using TN.Authentication.Infrastructure;
using TN.Inventory.Application;
using TN.Inventory.Infrastructure;
using TN.Purchase.Application;
using TN.Purchase.Infrastructure;
using TN.Reports.Infrastructure;
using TN.Sales.Application;
using TN.Sales.Infrastructure;
using TN.Setup.Application;
using TN.Setup.Infrastructure;
using TN.Shared.Application;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Infrastructure;
using TN.Shared.Infrastructure.ActivityProcessServices;
using TN.Shared.Infrastructure.BackGroundServices;
using TN.Shared.Infrastructure.CustomMiddleware.FiscalYearContext;
using TN.Shared.Infrastructure.CustomMiddleware.GlobalErrorHandling;
using TN.Shared.Infrastructure.Repository;
using TN.Shared.Infrastructure.SignalRHub;
using TN.Transactions.Application;
using TN.Transactions.Infrastructure;
using TN.Web.Configs;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region CORS Enable
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", corsBuilder =>
    {
        corsBuilder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials();
    });
});

#endregion

ConfigurationManager configuration = builder.Configuration;


//"message" and "Message" will both map to Message.
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.IncludeFields = true;
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Use PascalCase
    });






builder.Services
    .AddSharedApplication()
    .AddSharedInfrastructure(configuration);
builder.Services
    .AddAuthenticationApplication()
    .AddAuthenticationInfrastructure();

builder.Services
    .AddSetupApplication()
    .AddSetupInfrastructure();

builder.Services
    .AddAccountApplication()
    .AddAccountInfrastructure();

builder.Services
    .AddInventoryApplication()

    .AddInventoryInfrastructure();

builder.Services.
    AddPurchaseApplication()
    .AddPurchaseInfrastructure();

builder.Services
    .AddSalesApplication()
    .AddSalesInfrastructure();


builder.Services
   .AddReportsApplication()
   .AddReportsInfrastructure();

builder.Services
    .AddTransactionsApplication()
    .AddTransactionsInfrastructure();

builder.Services
    .AddPaymentApplication()
    .AddPaymentInfrastructure();



// Add SignalR
builder.Services.AddSignalR();

ApplicationBuilderConfig.Inject(builder);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


var app = builder.Build();

// Apply migrations during application startup
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    context.Database.Migrate();  // Apply any pending migrations
//}

//app.UseMiddleware<ExceptionMiddleware>();
ApplicationConfiguration.Inject(app);
app.UseStaticFiles();
app.UseErrorHandlingMiddleware();

app.UseRouting();

app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<FiscalContextMiddleware>();
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");
app.Run();
