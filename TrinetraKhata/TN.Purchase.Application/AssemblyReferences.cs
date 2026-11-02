using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Command.AddPurchaseDetails;
using TN.Purchase.Application.Purchase.Command.AddPurchaseItems;
using TN.Purchase.Application.Purchase.Command.QuotationToPurchase;
using TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationByCompany;
using TN.Purchase.Application.Purchase.Command.UpdatePurchaseDetails;
using TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnDetails;
using TN.Purchase.Application.PurchaseReturn.Command.UpdatePurchaseReturnDetails;

namespace TN.Purchase.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddPurchaseApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            //Fluent Validator
            services.AddScoped<IValidator<AddPurchaseItemsCommand>, AddPurchaseItemsCommandValidator>();
            services.AddScoped<IValidator<AddPurchaseDetailsCommand>, AddPurchaseDetailsCommandValidator>();
            services.AddScoped<IValidator<UpdatePurchaseDetailsCommand>, UpdatePurchaseDetailsCommandValidator>();
            services.AddScoped<IValidator<UpdateBillNumberGeneratorByCompanyCommand>, UpdateBillNumberGeneratorByCompanyCommandValidator>();
            services.AddScoped<IValidator<UpdatePurchaseReturnDetailsCommand>, UpdatePurchaseReturnDetailsCommandValidator>();
            services.AddScoped<IValidator<QuotationToPurchaseCommand>, QuotationToPurchaseCommandValidator>();



            services.AddScoped<IValidator<AddPurchaseReturnDetailsCommand>, AddPurchaseReturnDetailsCommandValidator>();

            return services;
        }
    }
}