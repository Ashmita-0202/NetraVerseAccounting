using Microsoft.Extensions.DependencyInjection;
using TN.Reports.Application.ServiceInterface;
using TN.Reports.Infrastructure.ServiceImpl;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Infrastructure.Repository;

namespace TN.Reports.Infrastructure
{
    public  static class AssemblyReferences
    {
        public static IServiceCollection AddReportsInfrastructure(this IServiceCollection services)
        {
            #region AutoMapper Coniguration
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion

            #region Inject Dependencies
            services.AddScoped<IAccountBookServices, AccountBookServices>();
            services.AddScoped<ILedgerBalanceServices, LedgerBalanceReportServices>();
            services.AddScoped<ITrialBalanceServices, TrialBalanceServices>();
            services.AddScoped<IProfitAndLossServices, ProfitAndLossReportServices>();
            services.AddScoped<IVatDetails, VatDetailsServices>();
            services.AddScoped<IBalanceSheetServices, BalanceSheetServices>();
            services.AddScoped<IPurchaseReportService, PurchaseReportService>();
            services.AddScoped<ISalesReportService, SalesReportService>();
            services.AddScoped<ITradingServices, TradingService>();
            services.AddScoped<IPartyStatementServices, PartyStatementServices>();
            services.AddScoped<IAnnexReportServices, AnnexReportServices>();
            services.AddScoped<IStockDetailReportService, StockDetailReportService>();
            #endregion

            return services;
        }
    }
}
