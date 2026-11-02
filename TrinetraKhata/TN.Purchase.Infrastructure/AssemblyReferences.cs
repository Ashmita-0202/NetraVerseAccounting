using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.ServiceInterface;
using TN.Purchase.Infrastructure.ServiceImpl;

namespace TN.Purchase.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddPurchaseInfrastructure(this IServiceCollection services)
        {

            #region AutoMapper Configuration
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #endregion
            services.AddScoped<IPurchaseDetailsServices, PurchaseDetailsServices>();
            services.AddScoped<IPurchaseItemsServices, PurchaseItemsServices>();
            return services;
        }
    }
}