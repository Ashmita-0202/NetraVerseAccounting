using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.ServiceInterface;
using TN.Sales.Infrastructure.ServiceImpl;

namespace TN.Sales.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddSalesInfrastructure(this IServiceCollection services)
        {
            #region AutoMapper Coniguration
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion

            #region Inject Dependencies

            //Using Scrutor external package for automatically inject into DI Container

            services.Scan(scan => scan.FromAssembliesOf(typeof(SalesDetailsServices))
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Services")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());



            //services.AddScoped<ISalesDetailsServices, SalesDetailsServices>();
            //services.AddScoped<ISalesReturnServices, SalesReturnServices>();
            #endregion

            return services;
        }
    }
}
