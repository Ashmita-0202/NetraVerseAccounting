using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.AutoMapper;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Infrastructure.ServiceImpl;
using TN.Shared.Infrastructure.DataSeed;

namespace TN.Setup.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddSetupInfrastructure(this IServiceCollection services)
        {

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //#region AutoMapper Configuration
            //var mappingConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new MappingProfile());
            //});

            //IMapper mapper = mappingConfig.CreateMapper();
            //services.AddSingleton(mapper);
            //#endregion

            #region Inject Dependency
            services.AddScoped<IProvinceServices, ProvinceServices>();
            services.AddTransient<DataSeeder>();
            services.AddScoped<IDistrictServices, DistrictServices>();
            services.AddScoped<IMunicipalityServices, MunicipalityServices>();

            services.AddScoped<IVdcServices, VdcServices>();
            

            services.AddScoped<IModule, ModuleService>();
            services.AddScoped<ISubModules, SubModulesServices>();
            services.AddScoped<IMenuServices, MenuServices>();
            services.AddScoped<IOrganizationServices, OrganizationServices>();
            services.AddScoped<IInstitutionServices, InstitutionServices>();
            services.AddScoped<ICompanyServices, CompanyServices>();

            services.AddTransient<IInitializeServices, InitializeServices>();



            #endregion
            return services;
        }
    }

   
}
