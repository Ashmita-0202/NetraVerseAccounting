using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.Shared.Command.CloseFiscalYear;
using TN.Shared.Application.Shared.Command.UpdateCurrentFiscalYear;
using TN.Shared.Application.Shared.Command.UpdateExpenseTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdateExpiredDateItemStatusByCompany;
using TN.Shared.Application.Shared.Command.UpdateIncomeTransactionNumberTypeCommand;
using TN.Shared.Application.Shared.Command.UpdateInventoryMethodByCompany;
using TN.Shared.Application.Shared.Command.UpdateItemStatusByCompany;
using TN.Shared.Application.Shared.Command.UpdateJournalRefByCompany;
using TN.Shared.Application.Shared.Command.UpdatePaymentTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdatePurchaseQuotationNumberType;
using TN.Shared.Application.Shared.Command.UpdatePurchaseRefNumberByCompany;
using TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType;
using TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdateSalesQuotationNumberType;
using TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberByCompany;
using TN.Shared.Application.Shared.Command.UpdateSalesReturnType;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInPurchase;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInSales;

namespace TN.Shared.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddSharedApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            #region FluentValidation
            services.AddScoped<IValidator<UpdateItemStatusByCompanyCommand>, UpdateItemStatusByCompanyCommandValidator>();
            services.AddScoped<IValidator<UpdateJournalRefByCompanyCommand>, UpdateJournalRefByCompanyCommandValidator>();
            services.AddScoped<IValidator<UpdateSalesReferenceNumberCommand>, UpdateSalesReferenceNumberCommandValidator>();
            services.AddScoped<IValidator<UpdateInventoryMethodCommand>, UpdateInventoryMethodCommandValidator>();
            services.AddScoped<IValidator<UpdatePurchaseReferenceNumberCommand>, UpdatePurchaseReferenceNumberCommandValidator>();
            services.AddScoped<IValidator<UpdateTaxStatusInPurchaseCommand>, UpdateTaxStatusInPurchaseCommandValidator>();
           services.AddScoped<IValidator<UpdateTaxStatusInSalesCommand>, UpdateTaxStatusInSalesCommandValidator>();
          services.AddScoped<IValidator<UpdateFiscalYearCommand>,UpdateFiscalYearCommandValidator>();
            services.AddScoped<IValidator<CloseFiscalYearCommand>, CloseFiscalYearCommandValidator>();
            services.AddScoped<IValidator<UpdateReceiptTransactionNumberTypeCommand>, UpdateReceiptTransactionNumberTypeCommandValidator>();
            services.AddScoped<IValidator<UpdatePaymentTransactionNumberTypeCommand>, UpdatePaymentTransactionNumberTypeCommandValidator>();
           services.AddScoped<IValidator<UpdateExpenseTransactionNumberTypeCommand>,UpdateExpenseTransactionNumberTypeCommandValidator>();
            services.AddScoped<IValidator<UpdateIncomeTransactionNumberTypeCommand>, UpdateIncomeTransactionNumberTypeCommandValidation>();
            services.AddScoped<IValidator<UpdatePurchaseReturnTypeCommand>, UpdatePurchaseReturnTypeCommandValidator>();
         services.AddScoped<IValidator<UpdateSalesReturnTypeCommand>, UpdateSalesReturnTypeCommandValidator>();
            services.AddScoped<IValidator<UpdatePurchaseQuotationTypeCommand>, UpdatePurchaseQuotationTypeCommandValidator>();
            services.AddScoped<IValidator<UpdateSalesQuotationTypeCommand>, UpdateSalesQuotationTypeCommandValidator>();

            #endregion




            return services;
        }
    }
}
