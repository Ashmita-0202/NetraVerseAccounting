
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TN.Account.Application.Account.Command.AddCustomer;
using TN.Account.Application.Account.Command.AddCustomerCategory;
using TN.Account.Application.Account.Command.AddJournalEntry;
using TN.Account.Application.Account.Command.AddJournalEntryDetails;
using TN.Account.Application.Account.Command.AddLedger;
using TN.Account.Application.Account.Command.AddLedgerGroup;
using TN.Account.Application.Account.Command.AddSubledgerGroup;
using TN.Account.Application.Account.Command.BillSundry;
using TN.Account.Application.Account.Command.ImportExcelForLedgers;
using TN.Account.Application.Account.Command.UpdateBillSundry;
using TN.Account.Application.Account.Command.UpdateCustomer;
using TN.Account.Application.Account.Command.UpdateCustomerCategory;
using TN.Account.Application.Account.Command.UpdateJournalEntry;
using TN.Account.Application.Account.Command.UpdateJournalEntryDetails;
using TN.Account.Application.Account.Command.UpdateLedger;
using TN.Account.Application.Account.Command.UpdateLedgerGroup;
using TN.Account.Application.Account.Command.UpdateMaster;
using TN.Account.Application.Account.Command.UpdateSubledgerGroup;
using TN.Inventory.Application.Inventory.Command.ImportExcelForItems;
using TN.Shared.Domain.Entities.OrganizationSetUp;

namespace TN.Account.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddAccountApplication(this IServiceCollection services)
        {
            services.AddMediatR(x=>x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            //Fluent Validator
            services.AddScoped<IValidator<AddLedgerGroupCommand>, AddLedgerGroupCommandValidator>();
            services.AddScoped<IValidator<UpdateLedgerGroupCommand>, UpdateLedgerGroupCommandValidator>();
            services.AddScoped<IValidator<AddLedgerCommand>, AddLedgerCommandValidator>();
            services.AddScoped<IValidator<UpdateLedgerCommand>, UpdateLedgerCommandValidator>();
            services.AddScoped<IValidator<UpdateMasterCommand>, UpdateMasterCommandValidator>();
            services.AddScoped<IValidator<AddCustomerCommand>, AddCustomerCommandValidator>();
            services.AddScoped<IValidator<AddCustomerCategoryCommand>, AddCustomerCategoryCommandValidator>();
            services.AddScoped<IValidator<UpdateCustomerCommand>, UpdateCustomerCommandValidator>();
            services.AddScoped<IValidator<UpdateCustomerCategoryCommand>, UpdateCustomerCategoryCommandValidator>();
            services.AddScoped<IValidator<AddJournalEntryCommand>, AddJournalEntityCommandValidator>();
            services.AddScoped<IValidator<AddJournalEntryDetailsCommand>, AddJournalEntryDetailsCommandValidator>();
            services.AddScoped<IValidator<UpdateJournalEntryCommand>, UpdateJournalEntryCommandValidator>();
           services.AddScoped<IValidator<UpdateJournalDetailsCommand>, UpdateJournalDetailsCommandValidator>();
            services.AddScoped<IValidator<UpdateSubledgerGroupCommand>, UpdateSubledgerGroupCommandValidator>();
            services.AddScoped<IValidator<AddSubledgerGroupCommand>, AddSubledgerGroupCommandValidator>();
            services.AddScoped<IValidator<LedgerExcelCommand>, LedgerExcelCommandValidator>();
            services.AddScoped<IValidator<AddBillSundryCommand>, AddBillSundryCommandValidator>();
            services.AddScoped<IValidator<UpdateBillSundryCommand>, UpdateBillSundryCommandValidator>();
            return services;

            

        }
        
    }

   
}
