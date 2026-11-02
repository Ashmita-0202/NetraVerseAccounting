using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Command.AddCompany;
using TN.Setup.Application.Setup.Command.AddInstitution;
using TN.Setup.Application.Setup.Command.AddMenu;
using TN.Setup.Application.Setup.Command.AddModule;
using TN.Setup.Application.Setup.Command.AddOrganization;
using TN.Setup.Application.Setup.Command.AddSubModules;
using TN.Setup.Application.Setup.Command.AssignMenusToRole;
using TN.Setup.Application.Setup.Command.AssignModulesToRole;
using TN.Setup.Application.Setup.Command.AssignSubModulesToRole;
using TN.Setup.Application.Setup.Command.UpdateAssignMenu;
using TN.Setup.Application.Setup.Command.UpdateAssignModules;
using TN.Setup.Application.Setup.Command.UpdateAssignSubModules;
using TN.Setup.Application.Setup.Command.UpdateBillNumberForPurchase;
using TN.Setup.Application.Setup.Command.UpdateCompany;
using TN.Setup.Application.Setup.Command.UpdateInstitution;
using TN.Setup.Application.Setup.Command.UpdateMenu;
using TN.Setup.Application.Setup.Command.UpdateModules;
using TN.Setup.Application.Setup.Command.UpdateOrganization;
using TN.Setup.Application.Setup.Command.UpdateSubModules;

namespace TN.Setup.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddSetupApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            //Fluent Validation
            services.AddScoped<IValidator<AddModuleCommand>,AddModuleCommandValidator>();
            services.AddScoped<IValidator<AssignModulesToRoleCommand>, AssignModulesToRoleValidator>();
            services.AddScoped<IValidator<AddSubModulesCommand>, AddSubModulesCommandValidator>();
            services.AddScoped<IValidator<AddMenuCommand>, AddMenuCommandValidator>();
            services.AddScoped<IValidator<UpdateModulesCommand>, UpdateModulesCommandValidators>();
            services.AddScoped<IValidator<AddOrganizationCommand>, AddOrganizationCommandValidator>();
            services.AddScoped<IValidator<UpdateOrganizationCommand>, UpdateOrganizationCommandValidator>();
            services.AddScoped<IValidator<AddInstitutionCommand>, AddInstitutionCommandValidator>();
            services.AddScoped<IValidator<UpdateInstitutionCommand>, UpdateInstitutionCommandValidator>();
            services.AddScoped<IValidator<AddCompanyCommand>, AddCompanyCommandValidator>();
            services.AddScoped<IValidator<UpdateCompanyCommand>, UpdateCompanyCommandValidator>();
            services.AddScoped<IValidator<UpdateSubModulesCommand>, UpdateSubModulesCommandValidator>();
            services.AddScoped<IValidator<UpdateMenuCommand>, UpdateMenuCommandValidator>();
            services.AddScoped<IValidator<AssignSubModulesToRoleCommand>, AssignSubModulesToRoleValidator>();
            services.AddScoped<IValidator<AssignMenusToRoleCommands>, AssignMenuToRolesValidator>();
            services.AddScoped<IValidator<UpdateAssignModulesCommand>, UpdateAssignModulesCommandValidator>();
            services.AddScoped<IValidator<UpdateAssignSubModulesCommand>, UpdateAssignSubModulesCommandValidator>();
            services.AddScoped<IValidator<UpdateAssignMenuCommand>, UpdateAssignMenuCommandValidator>();
            services.AddScoped<IValidator<UpdateBillNumberStatusForPurchaseCommand>, UpdateBillNumberStatusForPurchaseCommandValidator>();
            return services;
        }
    }
}
