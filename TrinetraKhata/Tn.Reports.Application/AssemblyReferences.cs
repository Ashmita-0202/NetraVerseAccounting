using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Tn.Reports.Application
{
    public static class AssemblyReferences
    {

       
            public static IServiceCollection AddReportsApplication(this IServiceCollection services)
            {
                services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

                //fluent validation
                
                return services;
            }
        }


    
}
