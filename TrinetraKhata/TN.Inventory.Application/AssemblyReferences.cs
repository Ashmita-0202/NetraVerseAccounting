using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.AddConversionFactor;
using TN.Inventory.Application.Inventory.Command.AddItemGroup;
using TN.Inventory.Application.Inventory.Command.AddItems;
using TN.Inventory.Application.Inventory.Command.AddStockAdjustment;
using TN.Inventory.Application.Inventory.Command.AddStockCenter;
using TN.Inventory.Application.Inventory.Command.AddStockTransferDetails;
using TN.Inventory.Application.Inventory.Command.AddUnits;
using TN.Inventory.Application.Inventory.Command.ImportExcelForItems;
using TN.Inventory.Application.Inventory.Command.UpdateConversionFactor;
using TN.Inventory.Application.Inventory.Command.UpdateItem;
using TN.Inventory.Application.Inventory.Command.UpdateItemGroup;
using TN.Inventory.Application.Inventory.Command.UpdateStockAdjustment;
using TN.Inventory.Application.Inventory.Command.UpdateStockCenter;
using TN.Inventory.Application.Inventory.Command.UpdateStockTransferDetails;
using TN.Inventory.Application.Inventory.Command.UpdateUnits;

namespace TN.Inventory.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddInventoryApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            //fluent validation
            services.AddScoped<IValidator<AddUnitsCommand>, AddUnitsCommandValidator>();
            services.AddScoped<IValidator<UpdateUnitsCommand>, UpdateUnitsCommandValidator>();
            services.AddScoped<IValidator<AddConversionFactorCommand>, AddConversionFactorCommandValidator>();
            services.AddScoped<IValidator<UpdateConversionFactorCommand>, UpdateConversionFactorCommandValidator>();
            services.AddScoped<IValidator<AddItemGroupCommand>,AddItemGroupCommandValidator>();
            services.AddScoped<IValidator<UpdateItemGroupCommand>, UpdateItemGroupCommandValidator>();
            services.AddScoped<IValidator<AddItemCommand>, AddItemCommandValidator>();
            services.AddScoped<IValidator<UpdateItemCommand>, UpdateItemCommandValidator > ();
            services.AddScoped<IValidator<ItemsExcelCommand>, ItemsExcelCommandValidator>();
           services.AddScoped<IValidator<AddStockCenterCommand>, AddStockCenterCommandValidator>();
           services.AddScoped<IValidator<UpdateStockCenterCommand>, UpdateStockCenterCommandValidator>();
            services.AddScoped<IValidator<UpdateStockAdjustmentCommand>, UpdateStockAdjustmentCommandValidator>();
            services.AddScoped<IValidator<AddStockAdjustmentCommand>, AddStockAdjustmentCommandValidator>();
            services.AddScoped<IValidator<AddStockTransferCommand>, AddStockTransferCommandValidator>();
            services.AddScoped<IValidator<UpdateStockTransferDetailsCommand>, UpdateStockTransferDetailsCommandValidator>();
      
            return services;
        }
    }
}
