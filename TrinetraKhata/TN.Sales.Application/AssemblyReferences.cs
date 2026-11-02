using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.Sales.Command.AddSalesDetails;
using TN.Sales.Application.Sales.Command.QuotationToSales;
using TN.Sales.Application.Sales.Command.UdpateBIllNumberGenerationByCompany;
using TN.Sales.Application.Sales.Command.UpdateSalesDetails;
using TN.Sales.Application.SalesReturn.Command.AddSalesReturnDetails;
using TN.Sales.Application.SalesReturn.Command.UpdateSalesReturnDetails;

namespace TN.Sales.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddSalesApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            //fluent validation
            services.AddScoped<IValidator<AddSalesDetailsCommand>, AddSalesDetailsCommandValidator>();
            services.AddScoped<IValidator<UpdateSalesDetailsCommand>, UpdateSalesDetailsCommandValidator>();
            services.AddScoped<IValidator<UpdateBillNumberGenerationByCompanyCommand>, UpdateBillNumberGenerationByCompanyCommandValidator>();
            services.AddScoped<IValidator<AddSalesReturnDetailsCommand>, AddSalesReturnDetailsCommandValidator>();
            services.AddScoped<IValidator<UpdateSalesReturnDetailsCommand>, UpdateSalesReturnDetailsCommandValidator>();
            services.AddScoped<IValidator<QuotationToSalesCommand>, QuotationToSalesCommandValidators>();
            
            return services;
        }
    }
}
