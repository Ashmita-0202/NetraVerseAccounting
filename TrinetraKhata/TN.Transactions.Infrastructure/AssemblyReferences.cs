using Microsoft.Extensions.DependencyInjection;
using TN.Transactions.Application.ServiceInterface;
using TN.Transactions.Infrastructure.ServiceImpl;

namespace TN.Transactions.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddTransactionsInfrastructure(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #region InjectDependency
            services.AddScoped<ITransactionsService, TransactionsService>();
            services.AddScoped<IReceiptServices, ReceiptServices>();
            services.AddScoped<IIncomeService, IncomeService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IPaymentsServices, PaymentsServices>();
            #endregion
            return services;
        }
    }
}
