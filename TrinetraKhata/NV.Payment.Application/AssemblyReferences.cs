using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NV.Payment.Application.Payment.Command.AddPayment;
using NV.Payment.Application.Payment.Command.UpdatePayment;

namespace NV.Payment.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddPaymentApplication(this IServiceCollection services)
        {
            services.AddMediatR(x=>x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            //FluentValidation
            services.AddScoped<IValidator<AddPaymentMethodCommand>, AddPaymentMethodCommandValidator>();
           services.AddScoped<IValidator<UpdatePaymentMethodCommand>, UpdatePaymentMethodCommandValidator>();
            return services;
        }    
    }
}
