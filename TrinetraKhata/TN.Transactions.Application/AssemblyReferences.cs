using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TN.receiptDatas.Application.receiptDatas.Command.UpdateReceipt;
using TN.Transactions.Application.Transactions.Command.AddExpense;
using TN.Transactions.Application.Transactions.Command.AddIncome;
using TN.Transactions.Application.Transactions.Command.AddPayments;
using TN.Transactions.Application.Transactions.Command.AddReceipt;
using TN.Transactions.Application.Transactions.Command.AddTransactions;
using TN.Transactions.Application.Transactions.Command.ImportExcelForReceipt;
using TN.Transactions.Application.Transactions.Command.UpdateExpense;
using TN.Transactions.Application.Transactions.Command.UpdateIncome;
using TN.Transactions.Application.Transactions.Command.UpdatePayment;
using TN.Transactions.Application.Transactions.Command.UpdateTransactions;

namespace TN.Transactions.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddTransactionsApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            //fluent validation
            services.AddScoped<IValidator<AddTransactionsCommand>, AddTransactionsCommandValidator>();
           services.AddScoped<IValidator<UpdateTransactionsCommand>, UpdateTransactionsCommandValidator>();
            services.AddScoped<IValidator<AddReceiptCommand>, AddReceiptCommandValidator>();
           services.AddScoped<IValidator<UpdateReceiptCommand>, UpdateReceiptCommandValidator>();
            services.AddScoped<IValidator<AddPaymentsCommand>, AddPaymentsCommandValidators>();
            services.AddScoped<IValidator<AddIncomeCommand>, AddIncomeCommandValidator>();
            services.AddScoped<IValidator<AddExpenseCommand>, AddExpenseCommandValidator>();
            services.AddScoped<IValidator<UpdateIncomeCommand>, UpdateIncomeCommandValidator>();
            services.AddScoped<IValidator<UpdatePaymentCommand>, UpdatePaymentCommandValidator>();
            services.AddScoped<IValidator<UpdateExpenseCommand>, UpdateExpenseCommandValidator>();
            services.AddScoped<IValidator<ReceiptExceCommand>, ReceiptExcelCommandValidator>();
            return services;
        }
    }
}
