using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.ServiceInterface;
using TN.Inventory.Infrastructure.ServiceImpl;

namespace TN.Inventory.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddInventoryInfrastructure(this IServiceCollection services)
        {
            #region AutoMapper Coniguration
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion
            #region Inject Dependencies
            services.AddScoped<IUnitsServices, UnitsServices>();
            services.AddScoped<IConversionFactorServices, ConversionFactorServices>();
            services.AddScoped<IItemGroupServices, ItemGroupServices>();
            services.AddScoped<IItemsServices, ItemsServices>();
            services.AddScoped<IInventoriesServices, InventoriesServices>();
            services.AddScoped<IInventoryMethodType, InventoryMethodTypeServices>();
            services.AddScoped<IStockCenterService, StockCenterService>();
            services.AddScoped<IStockTransferDetailsServices, StockTransferDetailsServices>();
            #endregion

            return services;
        }
    }
}
