using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TN.Account.Application.Account.Command.UpdateLedgerGroup;
using TN.Account.Application.ServiceInterface;
using TN.Account.Infrastructure.ServiceImpl;

namespace TN.Account.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddAccountInfrastructure(this IServiceCollection services)
        {
            #region AutoMapper Configuration
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #endregion

            #region Inject Dependencies
            services.AddScoped<IMasterService, MasterService>();
            services.AddScoped<ILedgerGroupService, LedgerGroupService>();
            services.AddScoped<ILedgerService, LedgerService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerCategoryService, CustomerCategoryService>();
            services.AddScoped<IJournalServices, JournalServices>();
            services.AddScoped<IJournalDetailsServices, JournalDetailsServices>();
            services.AddScoped<IChartAccountServices, ChartAccountServices>();
            services.AddScoped<IOpeningStockService, OpeningStockService>();
            services.AddScoped<ISubledgerGroupService, SubledgerGroupService>();
            services.AddScoped<IOpeningClosingBalanceServices, OpeningClosingBalanceServices>();
            services.AddScoped<IBillSundryServices, BillSundryServices>();

            #endregion
            return services;
        }
    }
}
