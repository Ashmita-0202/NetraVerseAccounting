using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Abstraction;
using TN.Authentication.Application.Authentication.Commands.Register;
using TN.Authentication.Application.AutoMapper;
using TN.Authentication.Application.ServiceInterface;
using TN.Authentication.Infrastructure.JwtImpl;
using TN.Authentication.Infrastructure.ServiceImpl;

namespace TN.Authentication.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddAuthenticationInfrastructure(this IServiceCollection services)
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
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IJwtProviders, JwtProviders>();
            services.AddScoped<IOtpServices, OtpServices>();
        
            
            #endregion
            return services;
        }
    }
}
